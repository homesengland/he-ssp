using HE.InvestmentLoans.Common.Routing;

namespace HE.InvestmentLoans.WWW.Routing;

public class WorkflowActionCannotBePerformedException : Exception
{
    private WorkflowActionCannotBePerformedException(Trigger action, string explanation)
        : base($"Cannot perform {action} action. {explanation}.")
    {
    }

    private WorkflowActionCannotBePerformedException(string explanation)
        : base($"Cannot change state. {explanation}.")
    {
    }

    public static WorkflowActionCannotBePerformedException MethodHasNoState(Trigger action, string methodName)
    {
        return new WorkflowActionCannotBePerformedException(action, $"Controller method {methodName} has no state assigned. Consider adding [{nameof(WorkflowStateAttribute)}] to the method.");
    }

    public static WorkflowActionCannotBePerformedException CannotFindDestination(Trigger action, object desctinationState)
    {
        return new WorkflowActionCannotBePerformedException(action, $"Cannot find controller method assigned to state: {desctinationState}. Consider adding [{nameof(WorkflowStateAttribute)}({desctinationState.GetType().Name}.{desctinationState})] to controller method you want to be redirected to.");
    }

    public static WorkflowActionCannotBePerformedException CannotFindDestination(object desctinationState)
    {
        return new WorkflowActionCannotBePerformedException($"Cannot find controller method assigned to state: {desctinationState}. Consider adding [{nameof(WorkflowStateAttribute)}({desctinationState.GetType().Name}.{desctinationState})] to controller method you want to be redirected to.");
    }
}
