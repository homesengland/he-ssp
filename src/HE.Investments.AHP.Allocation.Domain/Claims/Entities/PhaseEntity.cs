using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
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
        MilestoneClaim completionMilestone,
        MilestoneClaim? acquisitionMilestone = null,
        MilestoneClaim? startOnSiteMilestone = null)
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

    public MilestoneClaim? AcquisitionMilestone { get; }

    public MilestoneClaim? StartOnSiteMilestone { get; }

    public MilestoneClaim CompletionMilestone { get; }

    public bool IsModified => _modificationTracker.IsModified;

    public static PhaseEntity New(
        PhaseId id,
        AllocationBasicInfo allocation,
        PhaseName name,
        NumberOfHomes numberOfHomes,
        BuildActivity buildActivityType,
        MilestoneClaim completionMilestone,
        MilestoneClaim? acquisitionMilestone = null,
        MilestoneClaim? startOnSiteMilestone = null)
    {
        return new PhaseEntity(
            id,
            allocation,
            name,
            numberOfHomes,
            buildActivityType,
            completionMilestone,
            acquisitionMilestone,
            startOnSiteMilestone);
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

    public bool CanMilestoneBeClaimed(MilestoneType milestoneType)
    {
        return milestoneType switch
        {
            MilestoneType.Acquisition => AcquisitionMilestone?.IsNotClaimed == true,
            MilestoneType.StartOnSite => AcquisitionMilestone?.IsClaimed == true && StartOnSiteMilestone?.IsNotClaimed == true,
            MilestoneType.Completion => CompletionMilestone.IsNotClaimed && (StartOnSiteMilestone.IsNotProvided() || StartOnSiteMilestone!.IsClaimed),
            _ => false,
        };
    }
}
