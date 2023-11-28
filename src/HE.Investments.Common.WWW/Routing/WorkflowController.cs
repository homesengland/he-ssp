using System.Reflection;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Utils;
using HE.Investments.Loans.Common.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HE.Investments.Common.WWW.Routing;

public abstract class WorkflowController<TState> : Controller
    where TState : struct, Enum
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var stateAttribute = CurrentActionStateAttribute(context.ActionDescriptor);

        if (stateAttribute is null)
        {
            await base.OnActionExecutionAsync(context, next);
            return;
        }

        if (stateAttribute.State is not TState current)
        {
            throw new ArgumentException($"Controller action and state mismatch. {ControllerContext.ActionDescriptor.ControllerName} inherits from {nameof(WorkflowController<TState>)}, and requires");
        }

        var routing = await Routing(current);
        var targetState = routing.CurrentState(current);

        if (targetState.Equals(current) && await routing.StateCanBeAccessed(current))
        {
            await base.OnActionExecutionAsync(context, next);
            return;
        }

        var workflowAction = GetWorkflowAction(targetState);
        context.Result = new RedirectToActionResult(workflowAction.ActionName, workflowAction.ControllerName.WithoutPrefix(), context.RouteData.Values.Skip(2));
    }

    public Task<IActionResult> Continue()
    {
        var currentStateAttribute = CurrentActionStateAttribute(ControllerContext.ActionDescriptor) ?? throw WorkflowActionCannotBePerformedException.MethodHasNoState(Trigger.Continue, ControllerContext.ActionDescriptor.ActionName);

        return Continue(currentStateAttribute.StateAs<TState>(), null);
    }

    public Task<IActionResult> Continue(object routeData)
    {
        var currentStateAttribute = CurrentActionStateAttribute(ControllerContext.ActionDescriptor) ?? throw WorkflowActionCannotBePerformedException.MethodHasNoState(Trigger.Continue, ControllerContext.ActionDescriptor.ActionName);

        return Continue(currentStateAttribute.StateAs<TState>(), routeData);
    }

    public Task<IActionResult> Continue(string redirect, object routeData)
    {
        if (redirect.IsProvided())
        {
            return Task.FromResult(Change(redirect, routeData));
        }

        return Continue(routeData);
    }

    public Task<IActionResult> ContinueWithRedirect(object routeData)
    {
        return Continue(Request.Query["redirect"].ToString(), routeData);
    }

    public IActionResult Change(string redirectToState, object routeData)
    {
        if (!Enum.TryParse<TState>(redirectToState, true, out var nextState))
        {
            throw new ArgumentException($"Cannot parse: \"{redirectToState}\" to {typeof(TState).Name}");
        }

        return ChangeState(nextState, routeData);
    }

    public Task<IActionResult> Continue(TState currentState, object? routeData)
    {
        return ChangeState(Trigger.Continue, currentState, routeData);
    }

    public Task<IActionResult> Back()
    {
        var currentStateAttribute = CurrentActionStateAttribute(ControllerContext.ActionDescriptor) ?? throw WorkflowActionCannotBePerformedException.MethodHasNoState(Trigger.Back, ControllerContext.ActionDescriptor.ActionName);

        return Back(currentStateAttribute.StateAs<TState>(), null);
    }

    public Task<IActionResult> Back(object routeData)
    {
        var currentStateAttribute = CurrentActionStateAttribute(ControllerContext.ActionDescriptor) ?? throw WorkflowActionCannotBePerformedException.MethodHasNoState(Trigger.Back, ControllerContext.ActionDescriptor.ActionName);

        return Back(currentStateAttribute.StateAs<TState>(), routeData);
    }

    public Task<IActionResult> Back(TState currentState, object? routeData)
    {
        return ChangeState(Trigger.Back, currentState, routeData);
    }

    protected abstract Task<IStateRouting<TState>> Routing(TState currentState, object? routeData = null);

    private static WorkflowStateAttribute? CurrentActionStateAttribute(ActionDescriptor actionDescriptor)
    {
        return actionDescriptor.FilterDescriptors
            .Where(x => x.Filter is WorkflowStateAttribute)
            .Select(x => x.Filter as WorkflowStateAttribute)
            .FirstOrDefault();
    }

    private static bool IsWorkflowGetMethod(MethodInfo method)
    {
        return method.GetCustomAttributes(typeof(WorkflowStateAttribute), false).Length > 0 && method.GetCustomAttributes(typeof(HttpGetAttribute), false).Length > 0;
    }

    private async Task<IActionResult> ChangeState(Trigger trigger, TState currentState, object? routeData)
    {
        var routing = await Routing(currentState, routeData);
        var nextState = await routing.NextState(trigger);

        return RedirectToState(nextState, routeData);
    }

    private IActionResult ChangeState(TState targetState, object routeData)
    {
        return RedirectToState(targetState, routeData);
    }

    private IActionResult RedirectToState(TState nextState, object? routeData)
    {
        var nextAction = GetWorkflowAction(nextState);

        if (routeData is null)
        {
            return RedirectToAction(nextAction.ActionName, nextAction.ControllerName.WithoutPrefix());
        }
        else
        {
            return RedirectToAction(nextAction.ActionName, nextAction.ControllerName.WithoutPrefix(), routeData);
        }
    }

    private WorkflowAction<TState> GetWorkflowAction(TState nextState)
    {
        return WorkflowGetMethodsFromAllControllers().FirstOrDefault(x => x.State.Equals(nextState)) ?? throw WorkflowActionCannotBePerformedException.CannotFindDestination(nextState);
    }

    private IEnumerable<WorkflowAction<TState>> WorkflowGetMethodsFromAllControllers()
    {
        return GetType()
            .Assembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(Controller)))
            .SelectMany(t => t.GetMethods())
            .Where(IsWorkflowGetMethod)
            .SelectMany(m =>
            {
                var attributeAssignedToMethods = m.GetCustomAttributes(typeof(WorkflowStateAttribute), false).Cast<WorkflowStateAttribute>();
                var workflowMethods = new List<WorkflowAction<TState>>();
                foreach (var workflowStateAttribute in attributeAssignedToMethods)
                {
                    if (workflowStateAttribute.State is TState state)
                    {
                        workflowMethods.Add(new WorkflowAction<TState>(new ControllerName(m.DeclaringType!.Name), m.Name, state));
                    }
                }

                return workflowMethods;
            })
            .ToList();
    }
}
