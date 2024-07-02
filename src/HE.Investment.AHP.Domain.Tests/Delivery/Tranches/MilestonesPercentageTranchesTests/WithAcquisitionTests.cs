using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.Tranches;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Tranches.MilestonesPercentageTranchesTests;

public class WithAcquisitionTests
{
    [Theory]
    [InlineData(0.95)]
    [InlineData(0.5)]
    [InlineData(0.0)]
    public void ShouldReturnNewInstanceWithAcquisition_WhenAcquisitionIsLessThanMaxTranche(decimal acquisition)
    {
        // given
        var milestonesPercentageTranches = new MilestonesPercentageTranches(null, null, null);
        var acquisitionPercentage = new WholePercentage(acquisition);

        // when
        var result = milestonesPercentageTranches.WithAcquisition(acquisitionPercentage);

        // then
        result.Acquisition.Should().Be(acquisitionPercentage);
    }

    [Theory]
    [InlineData(0.96)]
    [InlineData(1.0)]
    [InlineData(1.1)]
    public void ShouldThrowValidationError_WhenAcquisitionIsMoreThanMaxTranche(decimal acquisition)
    {
        // given
        var milestonesPercentageTranches = new MilestonesPercentageTranches(null, null, null);
        var acquisitionPercentage = new WholePercentage(acquisition);

        // when
        var result = () => milestonesPercentageTranches.WithAcquisition(acquisitionPercentage);

        // then
        result.Should().Throw<DomainValidationException>().WithMessage("Acquisition tranche must be 95% or less of the grant apportioned");
    }
}
