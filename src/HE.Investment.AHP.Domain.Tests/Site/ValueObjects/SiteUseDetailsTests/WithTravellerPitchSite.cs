using FluentAssertions;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.SiteUseDetailsTests;

public class WithTravellerPitchSite
{
    [Fact]
    public void ShouldThrowException_WhenIsForTravellerPitchSiteIsFalse()
    {
        // given
        var testCandidate = new SiteUseDetails(true, false);

        // when
        var withTravellerPitchSite = () => testCandidate.WithTravellerPitchSite(TravellerPitchSiteType.Transit);

        // then
        withTravellerPitchSite.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldReturnNewSiteUseDetails_WhenIsForTravellerPitchSiteIsTrue()
    {
        // given
        var testCandidate = new SiteUseDetails(false, true);

        // when
        var result = testCandidate.WithTravellerPitchSite(TravellerPitchSiteType.Transit);

        // then
        result.IsPartOfStreetFrontInfill.Should().BeFalse();
        result.IsForTravellerPitchSite.Should().BeTrue();
        result.TravellerPitchSiteType.Should().Be(TravellerPitchSiteType.Transit);
    }
}
