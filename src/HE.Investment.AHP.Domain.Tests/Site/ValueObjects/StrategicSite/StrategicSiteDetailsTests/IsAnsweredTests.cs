using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.StrategicSite.StrategicSiteDetailsTests;

public class IsAnsweredTests
{
    [Fact]
    public void ShouldReturnTrue_WhenNotAStrategicSite()
    {
        // given
        var details = new StrategicSiteDetails(false);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnTrue_WhenStrategicSite()
    {
        // given
        var details = new StrategicSiteDetails(true, new StrategicSiteName("some site name"));

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenNotProvided()
    {
        // given
        var details = new StrategicSiteDetails();

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenNameMissingForStrategicSite()
    {
        // given
        var details = new StrategicSiteDetails(true);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
