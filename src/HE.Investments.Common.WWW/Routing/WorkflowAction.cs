using HE.Investments.Common.WWW.Utils;

namespace HE.Investments.Common.WWW.Routing;

public class WorkflowAction<TState>
    where TState : Enum
{
    public WorkflowAction(ControllerName controllerName, string actionName, TState state)
    {
        ControllerName = controllerName;
        ActionName = actionName;
        State = state;
    }

    public ControllerName ControllerName { get; set; }

    public string ActionName { get; set; }

    public TState State { get; set; }
}
