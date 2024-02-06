using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.StrategicSite.StrategicSiteDetailsTests;

public class CreateTests
{
    [Fact]
    public void ShouldCreate_WhenIsNotAStrategicSite()
    {
        // given && when
        var result = StrategicSiteDetails.Create(false);

        // then
        result.IsStrategicSite.Should().BeFalse();
        result.SiteName.Should().BeNull();
    }

    [Fact]
    public void ShouldCreate_WhenIsStrategicSiteWithName()
    {
        // given
        var siteName = "sme test site";

        // when
        var result = StrategicSiteDetails.Create(true, siteName);

        // then
        result.IsStrategicSite.Should().BeTrue();
        result.SiteName.Should().NotBeNull();
        result.SiteName!.Value.Should().Be(siteName);
    }

    [Fact]
    public void ShouldThrowException_WhenIsStrategicSiteNotProvided()
    {
        // given && when
        var result = () => StrategicSiteDetails.Create();

        // then
        result.Should().Throw<DomainValidationException>();
    }
}
