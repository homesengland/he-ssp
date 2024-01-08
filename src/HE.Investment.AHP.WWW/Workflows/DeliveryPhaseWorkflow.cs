using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.WWW.Workflows;

public class DeliveryPhaseWorkflow : IStateRouting<DeliveryPhaseWorkflowState>
{
    private readonly bool _isUnregisteredBody;
    private readonly StateMachine<DeliveryPhaseWorkflowState, Trigger> _machine;

    public DeliveryPhaseWorkflow(DeliveryPhaseWorkflowState currentSiteWorkflowState, bool isUnregisteredBody)
    {
        _isUnregisteredBody = isUnregisteredBody;
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
        return Task.FromResult(CanBeAccessed(nextState));
    }

    public bool CanBeAccessed(DeliveryPhaseWorkflowState state)
    {
        return state switch
        {
            DeliveryPhaseWorkflowState.New => true,
            DeliveryPhaseWorkflowState.Name => true,
            DeliveryPhaseWorkflowState.Summary => true,
            DeliveryPhaseWorkflowState.AcquisitionMilestone => !_isUnregisteredBody,
            DeliveryPhaseWorkflowState.StartOnSiteMilestone => !_isUnregisteredBody,
            DeliveryPhaseWorkflowState.PracticalCompletionMilestone => true,
            DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp => _isUnregisteredBody,
            DeliveryPhaseWorkflowState.CheckAnswers => true,
            _ => false,
        };
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(DeliveryPhaseWorkflowState.Create)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.Details);

        _machine.Configure(DeliveryPhaseWorkflowState.Name)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.Details);

        _machine.Configure(DeliveryPhaseWorkflowState.Summary)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.AcquisitionMilestone, () => !_isUnregisteredBody)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.PracticalCompletionMilestone, () => _isUnregisteredBody);

        // TODO: fix back for summary
        _machine.Configure(DeliveryPhaseWorkflowState.AcquisitionMilestone)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.StartOnSiteMilestone)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.Summary);

        _machine.Configure(DeliveryPhaseWorkflowState.StartOnSiteMilestone)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.PracticalCompletionMilestone)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.AcquisitionMilestone);

        _machine.Configure(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.CheckAnswers, () => !_isUnregisteredBody)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp, () => _isUnregisteredBody)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.StartOnSiteMilestone, () => !_isUnregisteredBody)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.Summary, () => _isUnregisteredBody);

        _machine.Configure(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.CheckAnswers)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.PracticalCompletionMilestone);

        _machine.Configure(DeliveryPhaseWorkflowState.CheckAnswers)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.PracticalCompletionMilestone, () => !_isUnregisteredBody)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp, () => _isUnregisteredBody);
    }
}
