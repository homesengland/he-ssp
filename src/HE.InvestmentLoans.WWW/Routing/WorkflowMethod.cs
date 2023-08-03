using HE.InvestmentLoans.Common.Utils.ValueObjects;

namespace HE.InvestmentLoans.WWW.Routing;

public class WorkflowMethod<TState>
    where TState : Enum
{
    public WorkflowMethod(ControllerName controllerName, string actionName, TState state)
    {
        ControllerName = controllerName;
        ActionName = actionName;
        State = state;
    }

    public ControllerName ControllerName { get; set; }

    public string ActionName { get; set; }

    public TState State { get; set; }
}
