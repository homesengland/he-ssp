using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.Tranches;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Tranches.MilestonesPercentageTranchesTests;

public class WithStartOnSiteTests
{
    [Theory]
    [InlineData(0.95)]
    [InlineData(0.5)]
    [InlineData(0.0)]
    public void ShouldReturnNewInstanceWithStartOnSite_WhenStartOnSiteIsLessThanMaxTranche(decimal startOnSite)
    {
        // given
        var milestonesPercentageTranches = new MilestonesPercentageTranches(null, null, null);
        var startOnSitePercentage = new WholePercentage(startOnSite);

        // when
        var result = milestonesPercentageTranches.WithStartOnSite(startOnSitePercentage);

        // then
        result.StartOnSite.Should().Be(startOnSitePercentage);
    }

    [Theory]
    [InlineData(0.96)]
    [InlineData(1.0)]
    [InlineData(1.1)]
    public void ShouldThrowValidationError_WhenStartOnSiteIsMoreThanMaxTranche(decimal startOnSite)
    {
        // given
        var milestonesPercentageTranches = new MilestonesPercentageTranches(null, null, null);
        var startOnSitePercentage = new WholePercentage(startOnSite);

        // when
        var result = () => milestonesPercentageTranches.WithStartOnSite(startOnSitePercentage);

        // then
        result.Should().Throw<DomainValidationException>().WithMessage("Start on site tranche must be at max 95% or less of the grant apportioned");
    }
}
