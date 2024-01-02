using HE.Investment.AHP.Contract.Delivery;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Workflows;

public class DeliveryPhaseWorkflow : IStateRouting<DeliveryPhaseWorkflowState>
{
    public Task<DeliveryPhaseWorkflowState> NextState(Trigger trigger)
    {
        return Task.FromResult(DeliveryPhaseWorkflowState.Name);
    }

    public Task<bool> StateCanBeAccessed(DeliveryPhaseWorkflowState nextState)
    {
        return Task.FromResult(true);
    }
}
