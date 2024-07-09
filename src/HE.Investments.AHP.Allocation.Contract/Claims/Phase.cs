namespace HE.Investments.AHP.Allocation.Contract.Claims;

public record Phase(
    string Name,
    AllocationBasicInfo Allocation,
    IList<MilestoneClaim> MilestoneClaims);
