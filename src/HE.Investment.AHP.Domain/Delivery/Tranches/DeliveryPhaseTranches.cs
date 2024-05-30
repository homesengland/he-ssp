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
        bool amendmentsRequested,
        bool? claimMilestone,
        bool isOneTranche)
    {
        Id = id;
        ApplicationBasicInfo = applicationBasicInfo;
        _amendmentsRequested = amendmentsRequested;
        Percentages = percentages;
        ClaimMilestone = claimMilestone;
        IsOneTranche = isOneTranche;
    }

    public event EntityModifiedEventHandler? TranchesAmended;

    public DeliveryPhaseId Id { get; }

    public ApplicationBasicInfo ApplicationBasicInfo { get; }

    public MilestonesPercentageTranches Percentages { get; private set; }

    public bool IsOneTranche { get; }

    public bool? ClaimMilestone { get; private set; }

    public bool CanBeAmended => _amendmentsRequested && !IsOneTranche && ApplicationBasicInfo.Status.IsIn(ApplicationStatus.ReferredBackToApplicant);

    public bool IsModified => _modificationTracker.IsModified;

    public void ProvideAcquisitionTranche(string? acquisition)
    {
        CheckIfTranchesCanBeAmended();
        Percentages = _modificationTracker.Change(Percentages, Percentages.WithAcquisition(WholePercentage.FromString(acquisition)), UnClaimMilestone, _ => TranchesAmended?.Invoke());
    }

    public void ProvideStartOnSiteTranche(string? startOnSite)
    {
        CheckIfTranchesCanBeAmended();
        Percentages = _modificationTracker.Change(Percentages, Percentages.WithStartOnSite(WholePercentage.FromString(startOnSite)), UnClaimMilestone, _ => TranchesAmended?.Invoke());
    }

    public void ProvideCompletionTranche(string? completion)
    {
        CheckIfTranchesCanBeAmended();
        Percentages = _modificationTracker.Change(Percentages, Percentages.WithCompletion(WholePercentage.FromString(completion)), UnClaimMilestone, _ => TranchesAmended?.Invoke());
    }

    public void ClaimMilestones(bool? understandClaimingMilestones)
    {
        CheckIfTranchesCanBeAmended();

        if (!understandClaimingMilestones.GetValueOrDefault())
        {
            OperationResult.ThrowValidationError(
                "Tranches.SummaryOfDelivery.UnderstandClaimingMilestones",
                "You must confirm you understand this to continue");
        }

        if (!Percentages.IsSumUpTo100Percentage())
        {
            OperationResult.ThrowValidationError("Tranches", "Tranche proportions for this delivery phase must add to 100%");
        }

        ClaimMilestone = _modificationTracker.Change(ClaimMilestone, true, () => TranchesAmended?.Invoke());
    }

    public bool IsAnswered()
    {
        if (!CanBeAmended)
        {
            return true;
        }

        return Percentages.IsSumUpTo100Percentage() && ClaimMilestone.GetValueOrDefault();
    }

    public void UnClaimMilestone()
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
