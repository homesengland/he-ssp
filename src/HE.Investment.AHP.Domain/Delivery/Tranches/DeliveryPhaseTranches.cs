using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Domain.Common;
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
        MilestoneTranches = milestoneTranches.WithGrantApportioned(grantApportioned);
        IsOneTranche = isOneTranche;
    }

    public DeliveryPhaseId Id { get; }

    public ApplicationBasicInfo ApplicationBasicInfo { get; }

    public MilestoneTranches MilestoneTranches { get; private set; }

    public bool IsOneTranche { get; }

    public bool? ClaimMilestone { get; private set; }

    public bool AmendmentsRequested { get; }

    public bool IsModified => _modificationTracker.IsModified;

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

    public void ClaimMilestones(YesNoType yesNo)
    {
        CheckIfTranchesCanBeAmended();

        if (yesNo != YesNoType.Yes)
        {
            OperationResult.ThrowValidationError("UnderstandClaimingMilestones", "You must understand the claiming of milestones to proceed.");
        }

        var summary = CalculateSummary();
        if (!summary.IsSumUpTo)
        {
            OperationResult.ThrowValidationError("Tranche", "Tranche proportions for this delivery phase must add to 100%");
        }

        ClaimMilestone = true;
    }

    public SummaryOfDelivery CalculateSummary()
    {
        if (MilestoneTranches.GrantApportioned <= 0)
        {
            return SummaryOfDelivery.LackOfCalculation;
        }

        if (MilestoneTranches.IsAmendRequested)
        {
            return new SummaryOfDelivery(MilestoneTranches.GrantApportioned, MilestoneTranches.Acquisition, MilestoneTranches.StartOnSite, MilestoneTranches.Completion, false);
        }

        if (IsOneTranche)
        {
            return new SummaryOfDelivery(MilestoneTranches.GrantApportioned, null, null, MilestoneTranches.GrantApportioned);
        }

        var acquisitionMilestone = (MilestoneTranches.GrantApportioned * ApplicationBasicInfo.Programme.MilestoneFramework.AcquisitionPercentage).ToWholeNumberRoundFloor();
        var startOnSiteMilestone = (MilestoneTranches.GrantApportioned * ApplicationBasicInfo.Programme.MilestoneFramework.StartOnSitePercentage).ToWholeNumberRoundFloor();
        var completionMilestone = MilestoneTranches.GrantApportioned - acquisitionMilestone - startOnSiteMilestone;

        return new SummaryOfDelivery(MilestoneTranches.GrantApportioned, acquisitionMilestone, startOnSiteMilestone, completionMilestone);
    }

    public bool IsAnswered()
    {
        if (!AmendmentsRequested)
        {
            return true;
        }

        return MilestoneTranches.IsSumUpTo && ClaimMilestone.GetValueOrDefault();
    }

    private void CheckIfTranchesCanBeAmended()
    {
        if (!AmendmentsRequested || ApplicationBasicInfo.Status.IsNotIn(ApplicationStatus.ReferredBackToApplicant))
        {
            throw new DomainValidationException("Delivery Phase Tranches cannot be amendment");
        }
    }
}
