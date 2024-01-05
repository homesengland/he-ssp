namespace HE.Investments.Common.WWW.Routing;

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

    public static WorkflowActionCannotBePerformedException CannotFindDestination(Trigger action, object destinationState)
    {
        return new WorkflowActionCannotBePerformedException(action, $"Cannot find controller method assigned to state: {destinationState}. Consider adding [{nameof(WorkflowStateAttribute)}({destinationState.GetType().Name}.{destinationState})] to controller method you want to be redirected to.");
    }

    public static WorkflowActionCannotBePerformedException CannotFindDestination(object destinationState)
    {
        return new WorkflowActionCannotBePerformedException($"Cannot find controller method assigned to state: {destinationState}. Consider adding [{nameof(WorkflowStateAttribute)}({destinationState.GetType().Name}.{destinationState})] to controller method you want to be redirected to.");
    }
}
