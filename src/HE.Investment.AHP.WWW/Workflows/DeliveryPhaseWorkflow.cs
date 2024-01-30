using HE.Investment.AHP.Contract.Delivery;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.WWW.Workflows;

public class DeliveryPhaseWorkflow : IStateRouting<DeliveryPhaseWorkflowState>
{
    private readonly StateMachine<DeliveryPhaseWorkflowState, Trigger> _machine;

    private readonly DeliveryPhaseDetails _model;
    private readonly bool _isReadOnly;

    public DeliveryPhaseWorkflow(DeliveryPhaseWorkflowState currentSiteWorkflowState, DeliveryPhaseDetails model, bool isReadOnly)
    {
        _model = model;
        _isReadOnly = isReadOnly;
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

    public DeliveryPhaseWorkflowState CurrentState(DeliveryPhaseWorkflowState targetState)
    {
        if (_isReadOnly)
        {
            return DeliveryPhaseWorkflowState.CheckAnswers;
        }

        if (targetState != DeliveryPhaseWorkflowState.Start || _model.Status == SectionStatus.NotStarted)
        {
            return targetState;
        }

        return _model switch
        {
            { Name: var x } when x.IsNotProvided() => DeliveryPhaseWorkflowState.Name,
            { TypeOfHomes: var x } when x.IsNotProvided() => DeliveryPhaseWorkflowState.TypeOfHomes,
            { BuildActivityType: var x } when x.IsNotProvided() => DeliveryPhaseWorkflowState.BuildActivityType,
            { IsReconfiguringExistingNeeded: true } when _model.ReconfiguringExisting.IsNotProvided() => DeliveryPhaseWorkflowState.ReconfiguringExisting,
            { NumberOfHomes: var x } when x.IsNotProvided() || !IsNumberOfHomesCompleted() => DeliveryPhaseWorkflowState.AddHomes,
            { } when (_model.AcquisitionDate.IsNotProvided() || _model.AcquisitionPaymentDate.IsNotProvided()) && AllMilestonesAvailable() => DeliveryPhaseWorkflowState.AcquisitionMilestone,
            { } when (_model.StartOnSiteDate.IsNotProvided() || _model.StartOnSitePaymentDate.IsNotProvided()) && AllMilestonesAvailable() => DeliveryPhaseWorkflowState.StartOnSiteMilestone,
            { } when _model.PracticalCompletionDate.IsNotProvided() || _model.PracticalCompletionPaymentDate.IsNotProvided() => DeliveryPhaseWorkflowState.PracticalCompletionMilestone,
            { } when _model.IsAdditionalPaymentRequested.IsNotProvided() && IsUnregisteredBody() => DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp,
            _ => DeliveryPhaseWorkflowState.CheckAnswers,
        };
    }

    public bool CanBeAccessed(DeliveryPhaseWorkflowState state)
    {
        return state switch
        {
            DeliveryPhaseWorkflowState.Start => true,
            DeliveryPhaseWorkflowState.Create => true,
            DeliveryPhaseWorkflowState.Name => true,
            DeliveryPhaseWorkflowState.TypeOfHomes => true,
            DeliveryPhaseWorkflowState.BuildActivityType => true,
            DeliveryPhaseWorkflowState.ReconfiguringExisting => _model.IsReconfiguringExistingNeeded,
            DeliveryPhaseWorkflowState.AddHomes => true,
            DeliveryPhaseWorkflowState.SummaryOfDelivery => IsNumberOfHomesCompleted(),
            DeliveryPhaseWorkflowState.SummaryOfDeliveryEditable => IsNumberOfHomesCompleted(),
            DeliveryPhaseWorkflowState.SummaryOfDeliveryTranche => IsNumberOfHomesCompleted(),
            DeliveryPhaseWorkflowState.AcquisitionMilestone => AllMilestonesAvailable() && IsNumberOfHomesCompleted(),
            DeliveryPhaseWorkflowState.StartOnSiteMilestone => AllMilestonesAvailable() && IsNumberOfHomesCompleted(),
            DeliveryPhaseWorkflowState.PracticalCompletionMilestone => IsNumberOfHomesCompleted(),
            DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp => IsUnregisteredBody() && IsNumberOfHomesCompleted(),
            DeliveryPhaseWorkflowState.CheckAnswers => true,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null),
        };
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(DeliveryPhaseWorkflowState.Create)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.TypeOfHomes);

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
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.SummaryOfDelivery)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.BuildActivityType, () => !_model.IsReconfiguringExistingNeeded)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.ReconfiguringExisting, () => _model.IsReconfiguringExistingNeeded);

        _machine.Configure(DeliveryPhaseWorkflowState.SummaryOfDelivery)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.AcquisitionMilestone, AllMilestonesAvailable)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.PracticalCompletionMilestone, OnlyCompletionMilestoneAvailable)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.AddHomes);

        _machine.Configure(DeliveryPhaseWorkflowState.SummaryOfDeliveryTranche)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.SummaryOfDeliveryEditable)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.SummaryOfDeliveryEditable);

        _machine.Configure(DeliveryPhaseWorkflowState.AcquisitionMilestone)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.StartOnSiteMilestone)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.SummaryOfDelivery);

        _machine.Configure(DeliveryPhaseWorkflowState.StartOnSiteMilestone)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.PracticalCompletionMilestone)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.SummaryOfDelivery, OnlyCompletionMilestoneAvailable)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.AcquisitionMilestone, AllMilestonesAvailable);

        _machine.Configure(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.CheckAnswers, IsRegisteredBody)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp, IsUnregisteredBody)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.StartOnSiteMilestone, AllMilestonesAvailable)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.SummaryOfDelivery, OnlyCompletionMilestoneAvailable);

        _machine.Configure(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.CheckAnswers)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.PracticalCompletionMilestone);

        _machine.Configure(DeliveryPhaseWorkflowState.CheckAnswers)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.PracticalCompletionMilestone, IsRegisteredBody)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp, IsUnregisteredBody);
    }

    private bool AllMilestonesAvailable() => IsRegisteredBody() && !_model.IsOnlyCompletionMilestone;

    private bool OnlyCompletionMilestoneAvailable() => !AllMilestonesAvailable();

    private bool IsUnregisteredBody() => _model.IsUnregisteredBody;

    private bool IsRegisteredBody() => !IsUnregisteredBody();

    private bool IsNumberOfHomesCompleted() => _model.NumberOfHomes > 0;
}
