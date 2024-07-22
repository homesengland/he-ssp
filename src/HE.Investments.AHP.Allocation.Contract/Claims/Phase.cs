using HE.Investment.AHP.Contract.Delivery.Enums;

namespace HE.Investments.AHP.Allocation.Contract.Claims;

public record Phase(
    PhaseId Id,
    string Name,
    AllocationBasicInfo Allocation,
    string NumberOfHomes,
    BuildActivityType BuildActivityType,
    IList<MilestoneClaim> MilestoneClaims);
