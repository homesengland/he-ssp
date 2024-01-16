using HE.Investment.AHP.Contract.Delivery;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.WWW.Workflows;

public class DeliveryPhaseWorkflow : IStateRouting<DeliveryPhaseWorkflowState>
{
    private readonly StateMachine<DeliveryPhaseWorkflowState, Trigger> _machine;

    private readonly DeliveryPhaseDetails _model;

    public DeliveryPhaseWorkflow(DeliveryPhaseWorkflowState currentSiteWorkflowState, DeliveryPhaseDetails model)
    {
        _model = model;
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
            DeliveryPhaseWorkflowState.Create => true,
            DeliveryPhaseWorkflowState.Name => true,
            DeliveryPhaseWorkflowState.TypeOfHomes => true,
            DeliveryPhaseWorkflowState.BuildActivityType => true,
            DeliveryPhaseWorkflowState.ReconfiguringExisting => _model.IsReconfiguringExistingNeeded,
            DeliveryPhaseWorkflowState.AddHomes => true,
            DeliveryPhaseWorkflowState.Summary => true,
            DeliveryPhaseWorkflowState.AcquisitionMilestone => !_model.IsUnregisteredBody,
            DeliveryPhaseWorkflowState.StartOnSiteMilestone => !_model.IsUnregisteredBody,
            DeliveryPhaseWorkflowState.PracticalCompletionMilestone => true,
            DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp => _model.IsUnregisteredBody,
            DeliveryPhaseWorkflowState.CheckAnswers => true,
            _ => false,
        };
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(DeliveryPhaseWorkflowState.Create)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.TypeOfHomes);

        _machine.Configure(DeliveryPhaseWorkflowState.Summary)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.AcquisitionMilestone, () => !_model.IsUnregisteredBody)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.PracticalCompletionMilestone, () => _model.IsUnregisteredBody);

        _machine.Configure(DeliveryPhaseWorkflowState.Name)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.TypeOfHomes);

        _machine.Configure(DeliveryPhaseWorkflowState.TypeOfHomes)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.BuildActivityType)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.Name);

        _machine.Configure(DeliveryPhaseWorkflowState.BuildActivityType)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.ReconfiguringExisting, () => _model.IsReconfiguringExistingNeeded)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.AddHomes, () => !_model.IsReconfiguringExistingNeeded)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.TypeOfHomes);

        _machine.Configure(DeliveryPhaseWorkflowState.ReconfiguringExisting)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.AddHomes)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.BuildActivityType);

        _machine.Configure(DeliveryPhaseWorkflowState.AddHomes)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.AcquisitionMilestone, () => !_model.IsUnregisteredBody)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.PracticalCompletionMilestone, () => _model.IsUnregisteredBody)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.BuildActivityType, () => !_model.IsReconfiguringExistingNeeded)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.ReconfiguringExisting, () => _model.IsReconfiguringExistingNeeded);

        _machine.Configure(DeliveryPhaseWorkflowState.AcquisitionMilestone)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.StartOnSiteMilestone)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.AddHomes);

        _machine.Configure(DeliveryPhaseWorkflowState.StartOnSiteMilestone)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.PracticalCompletionMilestone)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.AddHomes, () => _model.IsUnregisteredBody)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.AcquisitionMilestone, () => !_model.IsUnregisteredBody);

        _machine.Configure(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.CheckAnswers, () => !_model.IsUnregisteredBody)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp, () => _model.IsUnregisteredBody)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.StartOnSiteMilestone, () => !_model.IsUnregisteredBody)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.AddHomes, () => _model.IsUnregisteredBody);

        _machine.Configure(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.CheckAnswers)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.PracticalCompletionMilestone);

        _machine.Configure(DeliveryPhaseWorkflowState.CheckAnswers)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.PracticalCompletionMilestone, () => !_model.IsUnregisteredBody)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp, () => _model.IsUnregisteredBody);
    }
}
