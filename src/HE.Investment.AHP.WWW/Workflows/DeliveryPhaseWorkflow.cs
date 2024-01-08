using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.WWW.Workflows;

public class DeliveryPhaseWorkflow : IStateRouting<DeliveryPhaseWorkflowState>
{
    private readonly StateMachine<DeliveryPhaseWorkflowState, Trigger> _machine;

    public DeliveryPhaseWorkflow(DeliveryPhaseWorkflowState currentDeliveryPhaseWorkflowState)
    {
        _machine = new StateMachine<DeliveryPhaseWorkflowState, Trigger>(currentDeliveryPhaseWorkflowState);
        ConfigureTransitions();
    }

    public async Task<DeliveryPhaseWorkflowState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        return _machine.State;
    }

    public Task<bool> StateCanBeAccessed(DeliveryPhaseWorkflowState nextState)
    {
        return Task.FromResult(true);
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(DeliveryPhaseWorkflowState.Name)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.TypeOfHomes);

        _machine.Configure(DeliveryPhaseWorkflowState.TypeOfHomes)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.AcquisitionMilestone)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.Name);
    }
}
