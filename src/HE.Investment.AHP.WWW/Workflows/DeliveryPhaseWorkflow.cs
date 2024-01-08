using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.WWW.Workflows;

public class DeliveryPhaseWorkflow : IStateRouting<DeliveryPhaseWorkflowState>
{
    private readonly StateMachine<DeliveryPhaseWorkflowState, Trigger> _machine;

    public DeliveryPhaseWorkflow(DeliveryPhaseWorkflowState currentSiteWorkflowState)
    {
        _machine = new StateMachine<DeliveryPhaseWorkflowState, Trigger>(currentSiteWorkflowState);
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
        _machine.Configure(DeliveryPhaseWorkflowState.Create)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.Details);

        _machine.Configure(DeliveryPhaseWorkflowState.Name)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.Details);

        _machine.Configure(DeliveryPhaseWorkflowState.AcquisitionMilestone)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.StartOnSiteMilestone);

        _machine.Configure(DeliveryPhaseWorkflowState.StartOnSiteMilestone)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.PracticalCompletionMilestone)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.AcquisitionMilestone);

        _machine.Configure(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.UnregisteredProviderFollowUp)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.StartOnSiteMilestone);
    }
}
