using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.Tranches;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Tranches.MilestonesPercentageTranchesTests;

public class IsSumUpTo100PercentageTests
{
    [Theory]
    [InlineData(0.95, 0.05, 0.0)]
    [InlineData(0.5, 0.5, 0.0)]
    [InlineData(0.0, 0.0, 1.0)]
    public void ShouldReturnTrue_WhenSumIsEqualTo100(decimal acquisition, decimal startOnSite, decimal completion)
    {
        // given
        var milestonesPercentageTranches =
            new MilestonesPercentageTranches(new WholePercentage(acquisition), new WholePercentage(startOnSite), new WholePercentage(completion));

        // when
        var result = milestonesPercentageTranches.IsSumUpTo100Percentage();

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(0.94, 0.05, 0.0)]
    [InlineData(0.5, 0.5, 0.1)]
    [InlineData(0.0, 0.0, 0.99)]
    public void ShouldReturnFalse_WhenSumIsNotEqualTo100(decimal acquisition, decimal startOnSite, decimal completion)
    {
        // given
        var milestonesPercentageTranches =
            new MilestonesPercentageTranches(new WholePercentage(acquisition), new WholePercentage(startOnSite), new WholePercentage(completion));

        // when
        var result = milestonesPercentageTranches.IsSumUpTo100Percentage();

        // then
        result.Should().BeFalse();
    }
}
