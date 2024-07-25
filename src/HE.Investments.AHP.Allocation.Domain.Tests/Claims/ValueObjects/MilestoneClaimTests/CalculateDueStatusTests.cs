using FluentAssertions;
using HE.Investments.AHP.Allocation.Domain.Claims.Enums;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.ValueObjects.MilestoneClaimTests;

public class CalculateDueStatusTests
{
    private readonly DateTime _today = new(2024, 07, 20, 0, 0, 0, DateTimeKind.Local);

    [Theory]
    [InlineData(15)]
    [InlineData(150)]
    public void ShouldReturnUndefinedStatus_WhenItIsMoreThan14DaysBeforeForecastClaimDate(int days)
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.Draft().WithForecastClaimDate(_today.AddDays(days)).Build();

        // when
        var result = testCandidate.CalculateDueStatus(_today);

        // then
        result.Should().Be(MilestoneDueStatus.Undefined);
    }

    [Theory]
    [InlineData(7)]
    [InlineData(14)]
    public void ShouldReturnDueSoonStatus_WhenItIsBetween14And7DaysBeforeForecastClaimDate(int days)
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.Draft().WithForecastClaimDate(_today.AddDays(days)).Build();

        // when
        var result = testCandidate.CalculateDueStatus(_today);

        // then
        result.Should().Be(MilestoneDueStatus.DueSoon);
    }

    [Theory]
    [InlineData(6)]
    [InlineData(-6)]
    public void ShouldReturnDueStatus_WhenItIsBetween6DaysBeforeAnd6DaysAfterForecastClaimDate(int days)
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.Draft().WithForecastClaimDate(_today.AddDays(days)).Build();

        // when
        var result = testCandidate.CalculateDueStatus(_today);

        // then
        result.Should().Be(MilestoneDueStatus.Due);
    }

    [Theory]
    [InlineData(-7)]
    [InlineData(-150)]
    public void ShouldReturnOverdueStatus_WhenItIsMoreThan6DaysAfterForecastClaimDate(int days)
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.Draft().WithForecastClaimDate(_today.AddDays(days)).Build();

        // when
        var result = testCandidate.CalculateDueStatus(_today);

        // then
        result.Should().Be(MilestoneDueStatus.Overdue);
    }
}
