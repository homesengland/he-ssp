using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Planning.LandRegistryDetailsTests;

public class CreateTests
{
    [Theory]
    [InlineData(null)]
    [InlineData(false)]
    public void ShouldCreate_WhenTitleNumberNotRegistered(bool? isLandRegistryTitleNumberRegistered)
    {
        // given && when
        var details = new LandRegistryDetails(isLandRegistryTitleNumberRegistered, null, null);

        // then
        AssertLandRegistryDetails(details, isLandRegistryTitleNumberRegistered, null, null);
    }

    [Fact]
    public void ShouldCreate_WhenTitleNumberRegistered()
    {
        // given && when
        var details = new LandRegistryDetails(true, null, null);

        // then
        AssertLandRegistryDetails(details, true, null, null);
    }

    [Fact]
    public void ShouldCreate_WhenTitleNumberProvided()
    {
        // given
        var landRegistryTitleNumber = new LandRegistryTitleNumber("12");

        // when
        var details = new LandRegistryDetails(true, landRegistryTitleNumber, null);

        // then
        AssertLandRegistryDetails(details, true, landRegistryTitleNumber, null);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldCreate_WhenIsGrantFundingForAllHomesCoveredByTitleNumberProvided(bool isGrantFundingForAllHomesCoveredByTitleNumber)
    {
        // given && when
        var details = new LandRegistryDetails(true, null, isGrantFundingForAllHomesCoveredByTitleNumber);

        // then
        AssertLandRegistryDetails(details, true, null, isGrantFundingForAllHomesCoveredByTitleNumber);
    }

    [Fact]
    public void ShouldThrowException_WhenTitleNumberProvidedForNotRegistered()
    {
        // given
        var landRegistryTitleNumber = new LandRegistryTitleNumber("12");

        // when
        var result = () => new LandRegistryDetails(false, landRegistryTitleNumber, null);

        // then
        result.Should().Throw<DomainValidationException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldThrowException_WhenIsGrantFundingForAllHomesCoveredByTitleNumberProvidedForNotRegistered(
        bool isGrantFundingForAllHomesCoveredByTitleNumber)
    {
        // given && when
        var result = () => new LandRegistryDetails(false, null, isGrantFundingForAllHomesCoveredByTitleNumber);

        // then
        result.Should().Throw<DomainValidationException>();
    }

    private static void AssertLandRegistryDetails(
        LandRegistryDetails details,
        bool? isLandRegistryTitleNumberRegistered,
        LandRegistryTitleNumber? titleNumber,
        bool? isGrantFundingForAllHomesCoveredByTitleNumber)
    {
        details.Should().NotBeNull();
        details.IsLandRegistryTitleNumberRegistered.Should().Be(isLandRegistryTitleNumberRegistered);
        details.TitleNumber.Should().Be(titleNumber);
        details.IsGrantFundingForAllHomesCoveredByTitleNumber.Should().Be(isGrantFundingForAllHomesCoveredByTitleNumber);
    }
}
