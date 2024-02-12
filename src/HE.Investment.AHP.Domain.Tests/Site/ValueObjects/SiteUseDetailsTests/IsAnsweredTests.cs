using FluentAssertions;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.SiteUseDetailsTests;

public class IsAnsweredTests
{
    [Theory]
    [InlineData(true, true, TravellerPitchSiteType.Permanent)]
    [InlineData(true, false, TravellerPitchSiteType.Undefined)]
    [InlineData(false, false, TravellerPitchSiteType.Undefined)]
    public void ShouldReturnTrue_WhenEverythingIsProvided(bool isPartOfStreetFrontInfill, bool isForTravellerPitchSite, TravellerPitchSiteType travellerPitchSiteType)
    {
        // given
        var testCandidate = new SiteUseDetails(isPartOfStreetFrontInfill, isForTravellerPitchSite, travellerPitchSiteType);

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(true, true, TravellerPitchSiteType.Undefined)]
    [InlineData(null, null, TravellerPitchSiteType.Undefined)]
    public void ShouldReturnFalse_WhenSomeDataIsMissing(bool? isPartOfStreetFrontInfill, bool? isForTravellerPitchSite, TravellerPitchSiteType travellerPitchSiteType)
    {
        // given
        var testCandidate = new SiteUseDetails(isPartOfStreetFrontInfill, isForTravellerPitchSite, travellerPitchSiteType);

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
