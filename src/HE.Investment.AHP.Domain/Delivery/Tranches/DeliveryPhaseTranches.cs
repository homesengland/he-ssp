using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Domain.Common;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.Tranches;

public class DeliveryPhaseTranches : IQuestion
{
    private readonly ModificationTracker _modificationTracker = new();

    private readonly bool _amendmentsRequested;

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
        _amendmentsRequested = amendmentsRequested;
        MilestoneTranches = milestoneTranches;
        GrantApportioned = grantApportioned;
        IsOneTranche = isOneTranche;
    }

    public DeliveryPhaseId Id { get; }

    public ApplicationBasicInfo ApplicationBasicInfo { get; }

    public MilestoneTranches MilestoneTranches { get; private set; }

    public bool IsOneTranche { get; }

    public decimal GrantApportioned { get; }

    public bool? ClaimMilestone { get; private set; }

    public bool CanBeAmended => _amendmentsRequested && ApplicationBasicInfo.Status.IsIn(ApplicationStatus.ReferredBackToApplicant);

    public bool IsModified => _modificationTracker.IsModified;

    public void ProvideAcquisitionTranche(string? acquisition)
    {
        CheckIfTranchesCanBeAmended();
        MilestoneTranches = _modificationTracker.Change(MilestoneTranches, MilestoneTranches.WithAcquisition(WholePercentage.FromString(acquisition)));
    }

    public void ProvideStartOnSiteTranche(string? startOnSite)
    {
        CheckIfTranchesCanBeAmended();
        MilestoneTranches = _modificationTracker.Change(MilestoneTranches, MilestoneTranches.WithStartOnSite(WholePercentage.FromString(startOnSite)));
    }

    public void ProvideCompletionTranche(string? completion)
    {
        CheckIfTranchesCanBeAmended();
        MilestoneTranches = _modificationTracker.Change(MilestoneTranches, MilestoneTranches.WithCompletion(WholePercentage.FromString(completion)));
    }

    public void ClaimMilestones(bool? understandClaimingMilestones)
    {
        CheckIfTranchesCanBeAmended();

        if (!understandClaimingMilestones.GetValueOrDefault())
        {
            OperationResult.ThrowValidationError("Tranches.SummaryOfDeliveryAmend.UnderstandClaimingMilestones", "You must confirm you understand this to continue");
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
        if (GrantApportioned <= 0)
        {
            return SummaryOfDelivery.LackOfCalculation;
        }

        if (IsOneTranche)
        {
            return new SummaryOfDelivery(GrantApportioned, null, null, GrantApportioned);
        }

        if (MilestoneTranches.IsAmendRequested)
        {
            var acquisitionMilestoneAmended = (GrantApportioned * MilestoneTranches.Acquisition?.Value ?? 0m).ToWholeNumberRoundFloor();
            var startOnSiteMilestoneAmended = (GrantApportioned * MilestoneTranches.StartOnSite?.Value ?? 0m).ToWholeNumberRoundFloor();
            var completionMilestoneAmended = (GrantApportioned * MilestoneTranches.Completion?.Value ?? 0m).ToWholeNumberRoundFloor();
            return new SummaryOfDelivery(GrantApportioned, acquisitionMilestoneAmended, startOnSiteMilestoneAmended, completionMilestoneAmended, false);
        }

        var acquisitionMilestone = (GrantApportioned * ApplicationBasicInfo.Programme.MilestoneFramework.AcquisitionPercentage).ToWholeNumberRoundFloor();
        var startOnSiteMilestone = (GrantApportioned * ApplicationBasicInfo.Programme.MilestoneFramework.StartOnSitePercentage).ToWholeNumberRoundFloor();
        var completionMilestone = GrantApportioned - acquisitionMilestone - startOnSiteMilestone;

        return new SummaryOfDelivery(GrantApportioned, acquisitionMilestone, startOnSiteMilestone, completionMilestone);
    }

    public bool IsAnswered()
    {
        if (!CanBeAmended)
        {
            return true;
        }

        return MilestoneTranches.IsSumUpTo && ClaimMilestone.GetValueOrDefault();
    }

    private void CheckIfTranchesCanBeAmended()
    {
        if (!CanBeAmended)
        {
            throw new DomainValidationException("Delivery Phase Tranches cannot be amended");
        }
    }
}
