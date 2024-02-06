using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.Tranches;

public class DeliveryPhaseTranches : IQuestion
{
    private readonly ModificationTracker _modificationTracker = new();

    public DeliveryPhaseTranches(
        DeliveryPhaseId id,
        ApplicationBasicInfo applicationBasicInfo,
        MilestoneTranches milestoneTranches,
        decimal grantApportioned,
        bool amendmentsRequested,
        bool isOneTranche)
    {
        Id = id;
        ApplicationBasicInfo = applicationBasicInfo;
        AmendmentsRequested = amendmentsRequested;
        GrantApportioned = grantApportioned;
        MilestoneTranches = milestoneTranches.WithGrantApportioned(GrantApportioned);
        IsOneTranche = isOneTranche;
    }

    public DeliveryPhaseId Id { get; }

    public ApplicationBasicInfo ApplicationBasicInfo { get; }

    public MilestoneTranches MilestoneTranches { get; private set; }

    public bool IsOneTranche { get; }

    public bool? ClaimMilestone { get; private set; }

    public bool AmendmentsRequested { get; }

    public bool IsModified => _modificationTracker.IsModified;

    public decimal GrantApportioned { get; }

    public void ProvideAcquisitionTranche(decimal? acquisition)
    {
        CheckIfTranchesCanBeAmended();
        MilestoneTranches = _modificationTracker.Change(MilestoneTranches, MilestoneTranches.WithAcquisition(acquisition));
    }

    public void ProvideStartOnSiteTranche(decimal? startOnSite)
    {
        CheckIfTranchesCanBeAmended();
        MilestoneTranches = _modificationTracker.Change(MilestoneTranches, MilestoneTranches.WithStartOnSite(startOnSite));
    }

    public void ProvideCompletionTranche(decimal? completion)
    {
        CheckIfTranchesCanBeAmended();
        MilestoneTranches = _modificationTracker.Change(MilestoneTranches, MilestoneTranches.WithCompletion(completion));
    }

    public void ClaimMilestones(MilestoneFramework milestoneFramework, YesNoType yesNo)
    {
        CheckIfTranchesCanBeAmended();

        if (yesNo != YesNoType.Yes)
        {
            OperationResult.ThrowValidationError("UnderstandClaimingMilestones", "You must understand the claiming of milestones to proceed.");
        }

        var summary = CalculateSummary(milestoneFramework);
        if (!summary.IsSumUpTo)
        {
            OperationResult.ThrowValidationError("Tranche", "Tranche proportions for this delivery phase must add to 100%");
        }

        ClaimMilestone = true;
    }

    public SummaryOfDelivery CalculateSummary(MilestoneFramework milestoneFramework)
    {
        if (GrantApportioned <= 0)
        {
            return SummaryOfDelivery.LackOfCalculation;
        }

        if (MilestoneTranches.IsAmendRequested)
        {
            return new SummaryOfDelivery(GrantApportioned, MilestoneTranches.Acquisition, MilestoneTranches.StartOnSite, MilestoneTranches.Completion, false);
        }

        if (IsOneTranche)
        {
            return new SummaryOfDelivery(GrantApportioned, null, null, GrantApportioned);
        }

        var acquisitionMilestone = (GrantApportioned * milestoneFramework.AcquisitionPercentage).ToWholeNumberRoundFloor();
        var startOnSiteMilestone = (GrantApportioned * milestoneFramework.StartOnSitePercentage).ToWholeNumberRoundFloor();
        var completionMilestone = GrantApportioned - acquisitionMilestone - startOnSiteMilestone;

        return new SummaryOfDelivery(GrantApportioned, acquisitionMilestone, startOnSiteMilestone, completionMilestone);
    }

    public bool IsAnswered()
    {
        if (!AmendmentsRequested)
        {
            return true;
        }

        return MilestoneTranches.IsAnswered() && ClaimMilestone.IsProvided();
    }

    private void CheckIfTranchesCanBeAmended()
    {
        if (!AmendmentsRequested || ApplicationBasicInfo.Status.IsNotIn(ApplicationStatus.ReferredBackToApplicant))
        {
            throw new DomainValidationException("Delivery Phase Tranches cannot be amendment");
        }
    }
}
