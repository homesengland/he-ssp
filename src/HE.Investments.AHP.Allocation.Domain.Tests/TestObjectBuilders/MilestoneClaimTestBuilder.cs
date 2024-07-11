using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;

public class MilestoneClaimTestBuilder : TestObjectBuilder<MilestoneClaimTestBuilder, MilestoneClaim>
{
    public MilestoneClaimTestBuilder(MilestoneClaim item)
        : base(item)
    {
    }

    protected override MilestoneClaimTestBuilder Builder => this;

    public static MilestoneClaimTestBuilder New() => new(new(
        MilestoneType.Completion,
        Domain.Claims.Enums.MilestoneStatus.Submitted,
        new GrantApportioned(100, 50),
        new ClaimDate(
            new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2022, 12, 12, 0, 0, 0, DateTimeKind.Utc)),
        null,
        null));
}
