using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Entities;

public sealed class PhaseEntity : DomainEntity
{
    private readonly ModificationTracker _modificationTracker = new();

    public PhaseEntity(
        PhaseId id,
        AllocationBasicInfo allocation,
        PhaseName name,
        NumberOfHomes numberOfHomes,
        BuildActivityType buildActivityType,
        MilestoneClaimBase? acquisitionMilestone,
        MilestoneClaimBase? startOnSiteMilestone,
        MilestoneClaimBase completionMilestone)
    {
        Id = id;
        Allocation = allocation;
        Name = name;
        NumberOfHomes = numberOfHomes;
        BuildActivityType = buildActivityType;
        AcquisitionMilestone = acquisitionMilestone;
        StartOnSiteMilestone = startOnSiteMilestone;
        CompletionMilestone = completionMilestone;
    }

    public PhaseId Id { get; }

    public AllocationBasicInfo Allocation { get; }

    public PhaseName Name { get; }

    public NumberOfHomes NumberOfHomes { get; }

    public BuildActivityType BuildActivityType { get; }

    public MilestoneClaimBase? AcquisitionMilestone { get; private set; }

    public MilestoneClaimBase? StartOnSiteMilestone { get; private set; }

    public MilestoneClaimBase CompletionMilestone { get; private set; }

    public bool IsModified => _modificationTracker.IsModified;

    public MilestoneClaimBase? GetMilestoneClaim(MilestoneType milestoneType)
    {
        return milestoneType switch
        {
            MilestoneType.Acquisition => AcquisitionMilestone,
            MilestoneType.StartOnSite => StartOnSiteMilestone,
            MilestoneType.Completion => CompletionMilestone,
            _ => throw new ArgumentOutOfRangeException(nameof(milestoneType), milestoneType, null),
        };
    }

    public void CancelMilestoneClaim(MilestoneType milestoneType)
    {
        var claim = GetMilestoneClaim(milestoneType)
                    ?? throw new NotFoundException(nameof(MilestoneClaim), milestoneType);

        ProvideMilestoneClaim(claim.Cancel());
    }

    public bool CanMilestoneBeClaimed(MilestoneType milestoneType)
    {
        return milestoneType switch
        {
            MilestoneType.Acquisition => AcquisitionMilestone?.IsSubmitted == false,
            MilestoneType.StartOnSite => AcquisitionMilestone?.IsSubmitted == true && StartOnSiteMilestone?.IsSubmitted == false,
            MilestoneType.Completion => !CompletionMilestone.IsSubmitted && (StartOnSiteMilestone.IsNotProvided() || StartOnSiteMilestone!.IsSubmitted),
            _ => false,
        };
    }

    public void ProvideMilestoneClaim(MilestoneClaimBase claim)
    {
        if (!CanMilestoneBeClaimed(claim.Type))
        {
            OperationResult.ThrowValidationError(nameof(MilestoneClaim), $"{claim.Type.GetDescription()} milestone cannot be claimed right now.");
        }

        switch (claim.Type)
        {
            case MilestoneType.Acquisition:
                AcquisitionMilestone = _modificationTracker.Change(AcquisitionMilestone, claim);
                break;
            case MilestoneType.StartOnSite:
                StartOnSiteMilestone = _modificationTracker.Change(StartOnSiteMilestone, claim);
                break;
            case MilestoneType.Completion:
                CompletionMilestone = _modificationTracker.Change(CompletionMilestone, claim);
                break;
            case MilestoneType.Undefined:
            default:
                throw new InvalidOperationException("Cannot provide Unknown claim type.");
        }
    }

    public void ProvideMilestoneClaimAchievementDate(
        MilestoneClaimBase claim,
        Programme.Contract.Programme programme,
        DateDetails? achievementDate,
        DateTime currentDate)
    {
        var previousMilestoneSubmissionDate = claim.Type switch
        {
            MilestoneType.Acquisition => null,
            MilestoneType.StartOnSite => AcquisitionMilestone?.ClaimDate.SubmissionDate,
            MilestoneType.Completion => StartOnSiteMilestone?.ClaimDate.SubmissionDate,
            _ => throw new InvalidOperationException("Cannot provide Unknown claim type."),
        };

        ProvideMilestoneClaim(claim.WithAchievementDate(achievementDate, programme, previousMilestoneSubmissionDate, currentDate));
    }
}
