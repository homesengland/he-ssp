using FluentAssertions;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.SiteUseDetailsTests;

public class WithSiteUseTests
{
    [Fact]
    public void ShouldResetTravellerPitchSiteType_WhenItWasPreviouslySet()
    {
        // given
        var testCandidate = new SiteUseDetails(true, true, TravellerPitchSiteType.Permanent);

        // when
        var result = testCandidate.WithSiteUse(false, false);

        // then
        result.IsPartOfStreetFrontInfill.Should().BeFalse();
        result.IsForTravellerPitchSite.Should().BeFalse();
        result.TravellerPitchSiteType.Should().Be(TravellerPitchSiteType.Undefined);
    }
}
