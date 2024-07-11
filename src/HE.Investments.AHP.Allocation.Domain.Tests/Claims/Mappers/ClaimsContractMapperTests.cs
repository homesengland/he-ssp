using System.Globalization;
using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Mappers;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Mappers;

public class ClaimsContractMapperTests : TestBase<ClaimsContractMapper>
{
    [Fact]
    public void ShouldReturnCorrectAllocationDetails_WhenDataIsValid()
    {
        // given
        var allocation = AllocationEntityTestBuilder.New()
            .WithListOfPhaseClaims([PhaseEntityTestBuilder.New().Build()])
            .Build();

        // when
        var result = TestCandidate.Map(allocation, new PaginationRequest(1));

        // then
        result.AllocationBasicInfo.Id.Should().Be(allocation.Id);
        result.AllocationBasicInfo.Name.Should().Be(allocation.Name.Value);
        result.AllocationBasicInfo.ReferenceNumber.Should().Be(allocation.ReferenceNumber.Value);
        result.AllocationBasicInfo.LocalAuthority.Should().Be(allocation.LocalAuthority.Name);
        result.AllocationBasicInfo.ProgrammeName.Should().Be(allocation.Programme.ShortName);
        result.AllocationBasicInfo.Tenure.Should().Be(allocation.Tenure.Value);
        result.PhaseList.Items.Count.Should().Be(1);
    }

    [Fact]
    public void ShouldExcludeNullMilestones_WhenMilestonesAreMissing()
    {
        // given
        var phase = PhaseEntityTestBuilder.New().Build();

        // when
        var result = TestCandidate.Map(phase);

        // then
        result.MilestoneClaims.Count.Should().Be(1);
        result.MilestoneClaims.Should().NotContain(x => x.Type == MilestoneType.Acquisition);
        result.MilestoneClaims.Should().NotContain(x => x.Type == MilestoneType.StartOnSite);
        result.MilestoneClaims.Should().Contain(x => x.Type == MilestoneType.Completion);
    }

    [Fact]
    public void ShouldReturnCorrectDateDetails_WhenForecastAndActualDatesAreProvided()
    {
        // given
        var milestoneClaim = MilestoneClaimTestBuilder
            .New()
            .Build();

        // when
        var result = TestCandidate.Map(MilestoneType.Completion, milestoneClaim);

        // then
        result!.ForecastClaimDate.Should().NotBeNull();
        result.AchievementDate.Should().NotBeNull();
        result.ForecastClaimDate.Day.Should().Be(milestoneClaim.ClaimDate.ForecastClaimDate.Day.ToString(CultureInfo.InvariantCulture));
        result.ForecastClaimDate.Month.Should().Be(milestoneClaim.ClaimDate.ForecastClaimDate.Month.ToString(CultureInfo.InvariantCulture));
        result.ForecastClaimDate.Year.Should().Be(milestoneClaim.ClaimDate.ForecastClaimDate.Year.ToString(CultureInfo.InvariantCulture));
        result.AchievementDate!.Day.Should().Be(milestoneClaim.ClaimDate.ActualClaimDate!.Value.Day.ToString(CultureInfo.InvariantCulture));
        result.AchievementDate!.Month.Should().Be(milestoneClaim.ClaimDate.ActualClaimDate!.Value.Month.ToString(CultureInfo.InvariantCulture));
        result.AchievementDate!.Year.Should().Be(milestoneClaim.ClaimDate.ActualClaimDate!.Value.Year.ToString(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void ShouldReturnNull_WhenMilestoneClaimIsNull()
    {
        // given && when
        var result = TestCandidate.Map(MilestoneType.Completion, null);

        // then
        result.Should().BeNull();
    }
}
