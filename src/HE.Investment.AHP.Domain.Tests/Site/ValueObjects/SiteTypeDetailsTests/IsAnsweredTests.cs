using FluentAssertions;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using SiteTypeDetails = HE.Investment.AHP.Domain.Site.ValueObjects.SiteTypeDetails;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.SiteTypeDetailsTests;

public class IsAnsweredTests
{
    [Fact]
    public void ShouldReturnTrue_WhenSiteTypeDetailsProvided()
    {
        // given
        var details = new SiteTypeDetails(SiteType.Brownfield, true, false);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenIsRegenerationSiteNotProvided()
    {
        // given
        var details = new SiteTypeDetails(SiteType.Brownfield, true);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenIsOnGreenBeltNotProvided()
    {
        // given
        var details = new SiteTypeDetails(SiteType.Brownfield, null, true);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenSiteTypeNotProvided()
    {
        // given
        var details = new SiteTypeDetails(null, false, true);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
