using HE.Investments.AHP.Allocation.Domain.Claims.Enums;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public sealed class CanceledMilestoneClaim : MilestoneClaim
{
    public CanceledMilestoneClaim(MilestoneClaim claim)
        : base(claim.Type, MilestoneStatus.Draft, claim.GrantApportioned, new ClaimDate(claim.ClaimDate.ForecastClaimDate), null, null)
    {
    }
}
