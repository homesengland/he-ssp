using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;

namespace HE.Investment.AHP.WWW.Models.AllocationClaim;

public record PhaseClaimModel(string Id, string Name, AllocationBasicInfo Allocation, MilestoneClaim Claim);
