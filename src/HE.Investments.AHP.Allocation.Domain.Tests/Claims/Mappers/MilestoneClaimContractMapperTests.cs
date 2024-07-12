using System.Globalization;
using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Mappers;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;
using ContractMilestoneStatus = HE.Investments.AHP.Allocation.Contract.Claims.Enum.MilestoneStatus;
using DomainMilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Mappers;

public class MilestoneClaimContractMapperTests : TestBase<MilestoneClaimContractMapper>
{
    [Fact]
    public void ShouldReturnCorrectDateDetails_WhenForecastAndActualDatesAreProvided()
    {
        // given
        var milestoneClaim = MilestoneClaimTestBuilder.New().Build();
        var phase = PhaseEntityTestBuilder.New().WithCompletionMilestone(milestoneClaim).Build();

        // when
        var result = TestCandidate.Map(MilestoneType.Completion, phase, DateTime.Today);

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
        // given
        var phase = PhaseEntityTestBuilder.New().WithAcquisitionMilestone(null).Build();

        // when
        var result = TestCandidate.Map(MilestoneType.Acquisition, phase, DateTime.Today);

        // then
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(DomainMilestoneStatus.Submitted, ContractMilestoneStatus.Submitted)]
    [InlineData(DomainMilestoneStatus.UnderReview, ContractMilestoneStatus.UnderReview)]
    [InlineData(DomainMilestoneStatus.Approved, ContractMilestoneStatus.Approved)]
    [InlineData(DomainMilestoneStatus.Rejected, ContractMilestoneStatus.Rejected)]
    [InlineData(DomainMilestoneStatus.Reclaimed, ContractMilestoneStatus.Reclaimed)]
    public void ShouldMapToTheSameStatus_WhenStatusCanBeMappedDirectly(DomainMilestoneStatus domainStatus, ContractMilestoneStatus expectedStatus)
    {
        // given
        var today = new DateTime(2024, 07, 12, 0, 0, 0, DateTimeKind.Local);
        var milestone = MilestoneClaimTestBuilder.New().WithStatus(domainStatus).Build();
        var phase = PhaseEntityTestBuilder.New()
            .WithCompletionMilestone(milestone)
            .Build();

        // when
        var result = TestCandidate.Map(MilestoneType.Completion, phase, today);

        // then
        result.Should().NotBeNull();
        result!.Status.Should().Be(expectedStatus);
    }

    [Theory]
    [InlineData(DomainMilestoneStatus.Undefined)]
    [InlineData(DomainMilestoneStatus.Draft)]
    public void ShouldMapToDueStatus_WhenStatusIs(DomainMilestoneStatus domainStatus)
    {
        // given
        var today = new DateTime(2024, 07, 12, 0, 0, 0, DateTimeKind.Local);
        var milestone = MilestoneClaimTestBuilder.New()
            .WithStatus(domainStatus)
            .WithForecastClaimDate(today.AddDays(-50))
            .Build();
        var phase = PhaseEntityTestBuilder.New()
            .WithCompletionMilestone(milestone)
            .Build();

        // when
        var result = TestCandidate.Map(MilestoneType.Completion, phase, today);

        // then
        result.Should().NotBeNull();
        result!.Status.Should().Be(ContractMilestoneStatus.Overdue);
    }
}
