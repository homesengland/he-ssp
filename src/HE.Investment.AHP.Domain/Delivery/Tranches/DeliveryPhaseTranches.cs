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

    private MilestonesPercentageTranches _percentages;

    public DeliveryPhaseTranches(
        DeliveryPhaseId id,
        ApplicationBasicInfo applicationBasicInfo,
        MilestonesPercentageTranches percentages,
        decimal grantApportioned,
        bool amendmentsRequested,
        bool isOneTranche)
    {
        Id = id;
        ApplicationBasicInfo = applicationBasicInfo;
        _amendmentsRequested = amendmentsRequested;
        GrantApportioned = grantApportioned;
        _percentages = percentages;
        IsOneTranche = isOneTranche;
    }

    public DeliveryPhaseId Id { get; }

    public ApplicationBasicInfo ApplicationBasicInfo { get; }

    public bool IsOneTranche { get; }

    public decimal GrantApportioned { get; }

    public bool? ClaimMilestone { get; private set; }

    public bool CanBeAmended => _amendmentsRequested && !IsOneTranche && ApplicationBasicInfo.Status.IsIn(ApplicationStatus.ReferredBackToApplicant);

    public bool IsModified => _modificationTracker.IsModified;

    public void ProvideAcquisitionTranche(string? acquisition)
    {
        CheckIfTranchesCanBeAmended();
        var percentages = UseMilestonesPercentageTranches();
        _percentages = _modificationTracker.Change(_percentages, percentages.WithAcquisition(WholePercentage.FromString(acquisition)));
    }

    public void ProvideStartOnSiteTranche(string? startOnSite)
    {
        CheckIfTranchesCanBeAmended();
        var percentages = UseMilestonesPercentageTranches();
        _percentages = _modificationTracker.Change(_percentages, percentages.WithStartOnSite(WholePercentage.FromString(startOnSite)));
    }

    public void ProvideCompletionTranche(string? completion)
    {
        CheckIfTranchesCanBeAmended();
        var percentages = UseMilestonesPercentageTranches();
        _percentages = _modificationTracker.Change(_percentages, percentages.WithCompletion(WholePercentage.FromString(completion)));
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

        if (!_percentages.IsSumUpTo100Percentage())
        {
            OperationResult.ThrowValidationError("Tranche", "Tranche proportions for this delivery phase must add to 100%");
        }

        ClaimMilestone = true;
    }

    public MilestonesPercentageTranches GetPercentageTranches()
    {
        if (IsOneTranche)
        {
            return MilestonesPercentageTranches.OnlyCompletion;
        }

        if (CanBeAmended && _percentages.IsAnyPercentageProvided())
        {
            return _percentages;
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

        return new MilestonesTranches(acquisition, startOnSite, completion);
    }

    public bool IsAnswered()
    {
        if (!CanBeAmended)
        {
            return true;
        }

        return _percentages.IsSumUpTo100Percentage() && ClaimMilestone.GetValueOrDefault();
    }

    private MilestonesPercentageTranches UseMilestonesPercentageTranches()
    {
        if (!_percentages.IsAnyPercentageProvided())
        {
            return new MilestonesPercentageTranches(
                new WholePercentage(ApplicationBasicInfo.Programme.MilestoneFramework.AcquisitionPercentage),
                new WholePercentage(ApplicationBasicInfo.Programme.MilestoneFramework.StartOnSitePercentage),
                new WholePercentage(ApplicationBasicInfo.Programme.MilestoneFramework.CompletionPercentage));
        }

        return _percentages;
    }

    private void CheckIfTranchesCanBeAmended()
    {
        if (!CanBeAmended)
        {
            throw new DomainValidationException("Delivery Phase Tranches cannot be amended");
        }
    }
}
