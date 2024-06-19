using System.Reflection;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Workflow;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Utils;
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

        if (!await routing.StateCanBeAccessed(current))
        {
            throw new NotFoundException("Page could not be accessed for current state of application.");
        }

        if (targetState.Equals(current))
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

    public Task<IActionResult> Continue(TState currentState, object? routeData)
    {
        return ChangeState(Trigger.Continue, currentState, routeData);
    }

    public Task<IActionResult> ContinueWithRedirect(object routeData)
    {
        var redirect = routeData.GetPropertyValue<string>("redirect") ?? Request.Query["redirect"].ToString();

        return Continue(redirect, routeData);
    }

    public Task<IActionResult> ContinueWithWorkflow(object routeData, string? workflow = null)
    {
        if (string.IsNullOrEmpty(workflow))
        {
            if (!Request.TryGetWorkflowQueryParameter(out var queryWorkflow))
            {
                return Continue(routeData);
            }

            workflow = queryWorkflow;
        }

        routeData.ExpandRouteValues(new { workflow });

        return Continue(routeData);
    }

    public IActionResult Change(string redirectToState, object routeData)
    {
        if (!Enum.TryParse<TState>(redirectToState, true, out var nextState))
        {
            throw new ArgumentException($"Cannot parse: \"{redirectToState}\" to {typeof(TState).Name}");
        }

        return ChangeState(nextState, routeData);
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

    protected IStateRouting<TState> CreateChangedFlowWorkflow(
        EncodedStateRouting<TState> workflow,
        TState currentState,
        Func<TState, EncodedStateRouting<TState>> changedWorkflowFactory)
    {
        if (Request.TryGetWorkflowQueryParameter(out var lastEncodedWorkflow))
        {
            var lastWorkflow = new EncodedWorkflow<TState>(lastEncodedWorkflow);
            var currentWorkflow = workflow.GetEncodedWorkflow();
            var changedState = currentWorkflow.GetNextChangedWorkflowState(currentState, lastWorkflow);

            return changedWorkflowFactory(changedState);
        }

        return workflow;
    }

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

    private RedirectToActionResult ChangeState(TState targetState, object routeData)
    {
        return RedirectToState(targetState, routeData);
    }

    private RedirectToActionResult RedirectToState(TState nextState, object? routeData)
    {
        routeData = AddOrganisationIdToRouteData(routeData);
        var nextAction = GetWorkflowAction(nextState);

        return RedirectToAction(nextAction.ActionName, nextAction.ControllerName.WithoutPrefix(), routeData);
    }

    private WorkflowAction<TState> GetWorkflowAction(TState nextState)
    {
        return WorkflowGetMethodsFromAllControllers().Find(x => x.State.Equals(nextState)) ?? throw WorkflowActionCannotBePerformedException.CannotFindDestination(nextState);
    }

    private List<WorkflowAction<TState>> WorkflowGetMethodsFromAllControllers()
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

    private object AddOrganisationIdToRouteData(object? routeData)
    {
        var organisationId = Request.GetOrganisationIdFromRoute();
        return routeData.ExpandRouteValues(new { organisationId });
    }
}
