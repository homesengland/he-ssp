using HE.Investments.AHP.Allocation.Contract;

namespace HE.Investment.AHP.WWW.Models.AllocationClaim;

public record PhaseClaimModel(string Id, string Name, AllocationBasicInfo Allocation, MilestoneClaimModel Claim)
{
    public string? IsConfirmed => Claim.IsConfirmed == true ? "checked" : null;
}
