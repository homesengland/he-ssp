using System.Reflection;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.WWW.Utils.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HE.InvestmentLoans.WWW.Routing;

public abstract class WorkflowController<TState> : Controller
    where TState : Enum
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var stateAttribute = CurrentActionStateAttribute(context.ActionDescriptor);

        if (stateAttribute is null)
        {
            await base.OnActionExecutionAsync(context, next);
            return;
        }

        if (stateAttribute.State is not TState state)
        {
            throw new ArgumentException($"Controller action and state mismatch. {ControllerContext.ActionDescriptor.ControllerName} inherits from {nameof(WorkflowController<TState>)}, and requires");
        }
        else
        {
            if (await Routing(state).StateCanBeAccessed(state))
            {
                await base.OnActionExecutionAsync(context, next);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
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

    public Task<IActionResult> Continue(TState currentState, object routeData)
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

    public Task<IActionResult> Back(TState currentState, object routeData)
    {
        return ChangeState(Trigger.Back, currentState, routeData);
    }

    protected abstract IStateRouting<TState> Routing(TState currentState);

    private async Task<IActionResult> ChangeState(Trigger trigger, TState currentState, object routeData)
    {
        var nextState = await Routing(currentState).NextState(trigger);

        var nextAction = WorkflowGetMethodsFromAllControllers().FirstOrDefault(x => x.State.Equals(nextState)) ?? throw WorkflowActionCannotBePerformedException.CannotFindDestination(trigger, nextState);

        if (routeData is null)
        {
            return RedirectToAction(nextAction.ActionName, nextAction.ControllerName.WithoutPrefix());
        }
        else
        {
            return RedirectToAction(nextAction.ActionName, nextAction.ControllerName.WithoutPrefix(), routeData);
        }
    }

    private IList<WorkflowAction<TState>> WorkflowGetMethodsFromAllControllers()
    {
        return Assembly
            .GetExecutingAssembly()
            .GetTypes()
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
                        workflowMethods.Add(new WorkflowAction<TState>(new ControllerName(m.DeclaringType.Name), m.Name, state));
                    }
                }

                return workflowMethods;
            })
            .ToList();
    }

    private bool IsWorkflowGetMethod(MethodInfo method)
    {
        return method.GetCustomAttributes(typeof(WorkflowStateAttribute), false).Length > 0 && method.GetCustomAttributes(typeof(HttpGetAttribute), false).Length > 0;
    }

    private WorkflowStateAttribute CurrentActionStateAttribute(ActionDescriptor actionDescriptor)
    {
        return actionDescriptor.FilterDescriptors
            .Where(x => x.Filter is WorkflowStateAttribute)
            .Select(x => x.Filter as WorkflowStateAttribute)
            .FirstOrDefault();
    }
}
