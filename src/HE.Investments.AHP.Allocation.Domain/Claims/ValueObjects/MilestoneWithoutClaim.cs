using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public sealed class MilestoneWithoutClaim : DraftMilestoneClaim
{
    public MilestoneWithoutClaim(MilestoneType milestoneType, GrantApportioned grantApportioned, DateTime forecastClaimDate)
        : base(milestoneType, grantApportioned, new ClaimDate(forecastClaimDate), null, null)
    {
    }

    public MilestoneWithoutClaim(DraftMilestoneClaim claim)
        : base(claim.Type, claim.GrantApportioned, new ClaimDate(claim.ClaimDate.ForecastClaimDate), null, null)
    {
    }

    public override MilestoneStatus Status => MilestoneStatus.ClaimDoesNotExist;
}
