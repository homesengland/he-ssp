using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Workflows;

public class DeliveryPhaseWorkflow : EncodedStateRouting<DeliveryPhaseWorkflowState>
{
    private readonly DeliveryPhaseDetails _model;

    public DeliveryPhaseWorkflow(DeliveryPhaseWorkflowState currentSiteWorkflowState, DeliveryPhaseDetails model, bool isLocked = false)
        : base(currentSiteWorkflowState, isLocked)
    {
        _model = model;
        ConfigureTransitions();
    }

    public override bool CanBeAccessed(DeliveryPhaseWorkflowState state, bool? isReadOnlyMode = null)
    {
        if (isReadOnlyMode ?? _model.Application.IsReadOnly)
        {
            return state == DeliveryPhaseWorkflowState.CheckAnswers;
        }

        return state switch
        {
            DeliveryPhaseWorkflowState.Create => true,
            DeliveryPhaseWorkflowState.Name => true,
            DeliveryPhaseWorkflowState.TypeOfHomes => true,
            DeliveryPhaseWorkflowState.NewBuildActivityType => IsNewBuildActivityType(),
            DeliveryPhaseWorkflowState.RehabBuildActivityType => !IsNewBuildActivityType(),
            DeliveryPhaseWorkflowState.ReconfiguringExisting => _model.IsReconfiguringExistingNeeded,
            DeliveryPhaseWorkflowState.AddHomes => true,
            DeliveryPhaseWorkflowState.SummaryOfDelivery => IsNumberOfHomesCompleted(),
            DeliveryPhaseWorkflowState.SummaryOfDeliveryTranche => IsNumberOfHomesCompleted() && (_model.Tranches?.ShouldBeAmended ?? false),
            DeliveryPhaseWorkflowState.AcquisitionMilestone => AllMilestonesAvailable() && IsNumberOfHomesCompleted(),
            DeliveryPhaseWorkflowState.StartOnSiteMilestone => AllMilestonesAvailable() && IsNumberOfHomesCompleted(),
            DeliveryPhaseWorkflowState.PracticalCompletionMilestone => IsNumberOfHomesCompleted(),
            DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp => IsUnregisteredBody() && IsNumberOfHomesCompleted(),
            DeliveryPhaseWorkflowState.CheckAnswers => true,
            _ => false,
        };
    }

    private void ConfigureTransitions()
    {
        Machine.Configure(DeliveryPhaseWorkflowState.Create)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.TypeOfHomes);

        Machine.Configure(DeliveryPhaseWorkflowState.Name)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.TypeOfHomes);

        Machine.Configure(DeliveryPhaseWorkflowState.TypeOfHomes)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.NewBuildActivityType, IsNewBuildActivityType)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.RehabBuildActivityType, () => !IsNewBuildActivityType())
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.Name);

        Machine.Configure(DeliveryPhaseWorkflowState.NewBuildActivityType)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.AddHomes)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.TypeOfHomes);

        Machine.Configure(DeliveryPhaseWorkflowState.RehabBuildActivityType)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.ReconfiguringExisting, () => _model.IsReconfiguringExistingNeeded)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.AddHomes, () => !_model.IsReconfiguringExistingNeeded)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.TypeOfHomes);

        Machine.Configure(DeliveryPhaseWorkflowState.ReconfiguringExisting)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.AddHomes)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.RehabBuildActivityType);

        Machine.Configure(DeliveryPhaseWorkflowState.AddHomes)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.SummaryOfDelivery)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.NewBuildActivityType, () => !_model.IsReconfiguringExistingNeeded && IsNewBuildActivityType())
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.RehabBuildActivityType, () => !_model.IsReconfiguringExistingNeeded && !IsNewBuildActivityType())
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.ReconfiguringExisting, () => _model.IsReconfiguringExistingNeeded);

        Machine.Configure(DeliveryPhaseWorkflowState.SummaryOfDelivery)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.AcquisitionMilestone, AllMilestonesAvailable)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.PracticalCompletionMilestone, OnlyCompletionMilestoneAvailable)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.AddHomes);

        Machine.Configure(DeliveryPhaseWorkflowState.SummaryOfDeliveryTranche)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.SummaryOfDelivery)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.SummaryOfDelivery);

        Machine.Configure(DeliveryPhaseWorkflowState.AcquisitionMilestone)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.StartOnSiteMilestone)
            .Permit(Trigger.Back, DeliveryPhaseWorkflowState.SummaryOfDelivery);

        Machine.Configure(DeliveryPhaseWorkflowState.StartOnSiteMilestone)
            .Permit(Trigger.Continue, DeliveryPhaseWorkflowState.PracticalCompletionMilestone)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.SummaryOfDelivery, OnlyCompletionMilestoneAvailable)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.AcquisitionMilestone, AllMilestonesAvailable);

        Machine.Configure(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.CheckAnswers, IsRegisteredBody)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp, IsUnregisteredBody)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.StartOnSiteMilestone, AllMilestonesAvailable)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.SummaryOfDelivery, OnlyCompletionMilestoneAvailable);

        Machine.Configure(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)
            .PermitIf(Trigger.Continue, DeliveryPhaseWorkflowState.CheckAnswers)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.PracticalCompletionMilestone);

        Machine.Configure(DeliveryPhaseWorkflowState.CheckAnswers)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.PracticalCompletionMilestone, IsRegisteredBody)
            .PermitIf(Trigger.Back, DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp, IsUnregisteredBody);
    }

    private bool AllMilestonesAvailable() => !_model.IsOnlyCompletionMilestone;

    private bool OnlyCompletionMilestoneAvailable() => _model.IsOnlyCompletionMilestone;

    private bool IsUnregisteredBody() => _model.IsUnregisteredBody;

    private bool IsRegisteredBody() => !IsUnregisteredBody();

    private bool IsNumberOfHomesCompleted() => _model.NumberOfHomes > 0;

    private bool IsNewBuildActivityType() => _model.TypeOfHomes is TypeOfHomes.NewBuild;
}
