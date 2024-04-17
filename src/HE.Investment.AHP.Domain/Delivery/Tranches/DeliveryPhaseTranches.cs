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
        MilestonesPercentageTranches percentages,
        decimal grantApportioned,
        bool amendmentsRequested,
        bool? claimMilestone,
        bool isOneTranche)
    {
        Id = id;
        ApplicationBasicInfo = applicationBasicInfo;
        _amendmentsRequested = amendmentsRequested;
        GrantApportioned = grantApportioned;
        PercentagesAmended = percentages;
        ClaimMilestone = claimMilestone;
        IsOneTranche = isOneTranche;
    }

    public DeliveryPhaseId Id { get; }

    public ApplicationBasicInfo ApplicationBasicInfo { get; }

    public MilestonesPercentageTranches PercentagesAmended { get; private set; }

    public bool IsOneTranche { get; }

    public decimal GrantApportioned { get; }

    public bool? ClaimMilestone { get; private set; }

    public bool CanBeAmended => _amendmentsRequested && !IsOneTranche && ApplicationBasicInfo.Status.IsIn(ApplicationStatus.ReferredBackToApplicant);

    public bool IsModified => _modificationTracker.IsModified;

    public void ProvideAcquisitionTranche(string? acquisition)
    {
        CheckIfTranchesCanBeAmended();
        var percentages = UseMilestonesPercentageTranches();
        PercentagesAmended =
            _modificationTracker.Change(PercentagesAmended, percentages.WithAcquisition(WholePercentage.FromString(acquisition)), UnClaimMilestone);
    }

    public void ProvideStartOnSiteTranche(string? startOnSite)
    {
        CheckIfTranchesCanBeAmended();
        var percentages = UseMilestonesPercentageTranches();
        PercentagesAmended =
            _modificationTracker.Change(PercentagesAmended, percentages.WithStartOnSite(WholePercentage.FromString(startOnSite)), UnClaimMilestone);
    }

    public void ProvideCompletionTranche(string? completion)
    {
        CheckIfTranchesCanBeAmended();
        var percentages = UseMilestonesPercentageTranches();
        PercentagesAmended =
            _modificationTracker.Change(PercentagesAmended, percentages.WithCompletion(WholePercentage.FromString(completion)), UnClaimMilestone);
    }

    public void ClaimMilestones(bool? understandClaimingMilestones)
    {
        CheckIfTranchesCanBeAmended();

        if (!understandClaimingMilestones.GetValueOrDefault())
        {
            OperationResult.ThrowValidationError(
                "Tranches.SummaryOfDeliveryAmend.UnderstandClaimingMilestones",
                "You must confirm you understand this to continue");
        }

        if (!PercentagesAmended.IsSumUpTo100Percentage())
        {
            OperationResult.ThrowValidationError("Tranche", "Tranche proportions for this delivery phase must add to 100%");
        }

        ClaimMilestone = _modificationTracker.Change(ClaimMilestone, true);
    }

    public MilestonesPercentageTranches GetPercentageTranches()
    {
        if (IsOneTranche)
        {
            return MilestonesPercentageTranches.OnlyCompletion;
        }

        if (CanBeAmended && PercentagesAmended.IsAnyPercentageProvided())
        {
            return PercentagesAmended;
        }

        return UseMilestonesPercentageTranches();
    }

    public MilestonesTranches CalculateTranches()
    {
        if (GrantApportioned <= 0)
        {
            return MilestonesTranches.LackOfCalculation;
        }

        var percentages = GetPercentageTranches();
        var acquisition = (GrantApportioned * percentages.Acquisition?.Value).ToWholeNumberRoundFloor();
        var startOnSite = (GrantApportioned * percentages.StartOnSite?.Value).ToWholeNumberRoundFloor();
        var completion = (GrantApportioned * percentages.Completion?.Value).RoundToTwoDecimalPlaces();

        var leftOver = (GrantApportioned - (acquisition + startOnSite + completion)).RoundToTwoDecimalPlaces();
        if (leftOver > 0 && (leftOver < GrantApportioned * 0.01m || leftOver < 1))
        {
            completion += leftOver;
        }

        var sumOfGrantApportioned = acquisition + startOnSite + completion;

        return new MilestonesTranches(sumOfGrantApportioned, acquisition, startOnSite, completion);
    }

    public bool IsAnswered()
    {
        if (!CanBeAmended)
        {
            return true;
        }

        return PercentagesAmended.IsSumUpTo100Percentage() && ClaimMilestone.GetValueOrDefault();
    }

    private MilestonesPercentageTranches UseMilestonesPercentageTranches()
    {
        if (!PercentagesAmended.IsAnyPercentageProvided())
        {
            return new MilestonesPercentageTranches(
                new WholePercentage(ApplicationBasicInfo.Programme.MilestoneFramework.AcquisitionPercentage),
                new WholePercentage(ApplicationBasicInfo.Programme.MilestoneFramework.StartOnSitePercentage),
                new WholePercentage(ApplicationBasicInfo.Programme.MilestoneFramework.CompletionPercentage));
        }

        return PercentagesAmended;
    }

    private void UnClaimMilestone()
    {
        ClaimMilestone = _modificationTracker.Change(ClaimMilestone, null);
    }

    private void CheckIfTranchesCanBeAmended()
    {
        if (!CanBeAmended)
        {
            throw new DomainValidationException("Delivery Phase Tranches cannot be amended");
        }
    }
}
