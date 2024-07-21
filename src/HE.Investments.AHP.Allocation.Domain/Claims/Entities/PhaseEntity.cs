using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using MilestoneClaim = HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects.MilestoneClaim;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Entities;

public class PhaseEntity : DomainEntity
{
    private readonly ModificationTracker _modificationTracker = new();

    public PhaseEntity(
        PhaseId id,
        AllocationBasicInfo allocation,
        PhaseName name,
        NumberOfHomes numberOfHomes,
        BuildActivity buildActivityType,
        MilestoneClaim? acquisitionMilestone,
        MilestoneClaim? startOnSiteMilestone,
        MilestoneClaim completionMilestone)
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

    public BuildActivity BuildActivityType { get; }

    public MilestoneClaim? AcquisitionMilestone { get; private set; }

    public MilestoneClaim? StartOnSiteMilestone { get; private set; }

    public MilestoneClaim CompletionMilestone { get; private set; }

    public bool IsModified => _modificationTracker.IsModified;

    public static PhaseEntity New(
        PhaseId id,
        AllocationBasicInfo allocation,
        PhaseName name,
        NumberOfHomes numberOfHomes,
        BuildActivity buildActivityType,
        MilestoneClaim? acquisitionMilestone,
        MilestoneClaim? startOnSiteMilestone,
        MilestoneClaim completionMilestone)
    {
        return new PhaseEntity(
            id,
            allocation,
            name,
            numberOfHomes,
            buildActivityType,
            acquisitionMilestone,
            startOnSiteMilestone,
            completionMilestone);
    }

    public MilestoneClaim? GetMilestoneClaim(MilestoneType milestoneType)
    {
        return milestoneType switch
        {
            MilestoneType.Acquisition => AcquisitionMilestone,
            MilestoneType.StartOnSite => StartOnSiteMilestone,
            MilestoneType.Completion => CompletionMilestone,
            _ => null,
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

    public void ProvideMilestoneClaim(MilestoneClaim claim)
    {
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

    public void ProvideMilestoneClaimAchievementDate(MilestoneClaim claim, Programme.Contract.Programme programme, DateDetails? achievementDate)
    {
        switch (claim.Type)
        {
            case MilestoneType.Acquisition:
                claim.WithAchievementDate(achievementDate, programme, null);
                break;
            case MilestoneType.StartOnSite:
                claim.WithAchievementDate(achievementDate, programme, AcquisitionMilestone?.ClaimDate.SubmissionDate);
                break;
            case MilestoneType.Completion:
                claim.WithAchievementDate(achievementDate, programme, StartOnSiteMilestone?.ClaimDate.SubmissionDate);
                break;
            case MilestoneType.Undefined:
            default:
                throw new InvalidOperationException("Cannot provide Unknown claim type.");
        }

        ProvideMilestoneClaim(claim);
    }
}
