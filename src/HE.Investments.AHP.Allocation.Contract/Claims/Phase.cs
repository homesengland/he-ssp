namespace HE.Investments.AHP.Allocation.Contract.Claims;

public record Phase(
    string Name,
    AllocationBasicInfo Allocation,
    MilestoneClaim? AcquisitionMilestone,
    MilestoneClaim? StartOnSiteMilestone,
    MilestoneClaim CompletionMilestone);
