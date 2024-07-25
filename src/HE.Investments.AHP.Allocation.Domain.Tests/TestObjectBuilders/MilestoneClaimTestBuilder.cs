using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.TestsUtils;
using HE.Investments.TestsUtils.TestFramework;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;

public class MilestoneClaimTestBuilder : TestObjectBuilder<MilestoneClaimTestBuilder, MilestoneClaimBase>
{
    private MilestoneClaimTestBuilder(MilestoneClaimBase item)
        : base(item)
    {
    }

    protected override MilestoneClaimTestBuilder Builder => this;

    public static MilestoneClaimTestBuilder Draft() => new(new DraftMilestoneClaim(
        MilestoneType.Completion,
        new GrantApportioned(100, 50),
        new ClaimDate(
            new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            DateDetails.FromDateTime(new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
            DateDetails.FromDateTime(new DateTime(2022, 12, 12, 0, 0, 0, DateTimeKind.Utc))),
        null,
        null));

    public MilestoneClaimTestBuilder WithoutClaim()
    {
        return new MilestoneClaimTestBuilder(new MilestoneWithoutClaim((DraftMilestoneClaim)Item));
    }

    public MilestoneClaimTestBuilder Submitted(MilestoneStatus? status = null)
    {
        return new(new SubmittedMilestoneClaim(
            Item.Type,
            status ?? MilestoneStatus.Approved,
            Item.GrantApportioned,
            Item.ClaimDate,
            Item.CostsIncurred,
            Item.IsConfirmed));
    }

    public MilestoneClaimTestBuilder WithType(MilestoneType value) => SetProperty(x => x.Type, value);

    public MilestoneClaimTestBuilder WithForecastClaimDate(DateTime value)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(Item.ClaimDate, nameof(ClaimDate.ForecastClaimDate), value);
        return this;
    }

    public MilestoneClaimTestBuilder WithMilestoneAchievedDate(DateDetails value)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(Item.ClaimDate, nameof(ClaimDate.AchievementDate), value);
        return this;
    }

    public MilestoneClaimTestBuilder WithMilestoneSubmissionDate(DateDetails value)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(Item.ClaimDate, nameof(ClaimDate.SubmissionDate), value);
        return this;
    }

    public MilestoneClaimTestBuilder WithGrantApportioned(decimal amount, decimal percentage)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(Item.GrantApportioned, nameof(GrantApportioned.Amount), amount);
        PrivatePropertySetter.SetPropertyWithNoSetter(Item.GrantApportioned, nameof(GrantApportioned.Percentage), percentage);
        return this;
    }

    public MilestoneClaimTestBuilder WithCostsIncurred(bool value) => SetProperty(x => x.CostsIncurred, value);

    public MilestoneClaimTestBuilder WithConfirmation(bool value) => SetProperty(x => x.IsConfirmed, value);
}
