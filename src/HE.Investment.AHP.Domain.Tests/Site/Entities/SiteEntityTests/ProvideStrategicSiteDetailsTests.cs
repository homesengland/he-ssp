using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;
using HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Site.Entities.SiteEntityTests;

public class ProvideStrategicSiteDetailsTests
{
    [Fact]
    public async Task ShouldSaveStrategicSiteName_WhenStrategicSiteNameIsProvided()
    {
        // given
        var details = new StrategicSiteDetails(true, new StrategicSiteName("Test Site"));
        var siteEntity = SiteEntityBuilder.New().Build();

        // when
        await siteEntity.ProvideStrategicSiteDetails(
            details,
            StrategicSiteNameExistMockBuilder.New().WithIsExistAsFalse().BuildObject(),
            CancellationToken.None);

        // when
        siteEntity.StrategicSiteDetails.Should().NotBeNull();
        siteEntity.StrategicSiteDetails!.IsStrategicSite.Should().BeTrue();
        siteEntity.StrategicSiteDetails.SiteName.Should().Be(new StrategicSiteName("Test Site"));
        siteEntity.IsModified.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldNotModifyEntity_WhenStrategicSiteNameHasNotChanged()
    {
        // given
        var details = new StrategicSiteDetails(true, new StrategicSiteName("Test Site"));
        var siteEntity = SiteEntityBuilder.New().WithStrategicDetails(new StrategicSiteDetails(true, new StrategicSiteName("Test Site"))).Build();

        // when
        await siteEntity.ProvideStrategicSiteDetails(
            details,
            StrategicSiteNameExistMockBuilder.New().WithIsExistAsTrue().BuildObject(),
            CancellationToken.None);

        // when
        siteEntity.StrategicSiteDetails.Should().NotBeNull();
        siteEntity.StrategicSiteDetails!.IsStrategicSite.Should().BeTrue();
        siteEntity.StrategicSiteDetails.SiteName.Should().Be(new StrategicSiteName("Test Site"));
        siteEntity.IsModified.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldModifyEntity_WhenStrategicSiteNameHasChangedToNull()
    {
        // given
        var siteEntity = SiteEntityBuilder.New().Build();

        // when
        await siteEntity.ProvideStrategicSiteDetails(
            null,
            StrategicSiteNameExistMockBuilder.New().WithIsExistAsTrue().BuildObject(),
            CancellationToken.None);

        // when
        siteEntity.StrategicSiteDetails.Should().BeNull();
        siteEntity.IsModified.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldThrowException_WhenStrategicSiteNameIsAlreadyExist()
    {
        // given
        var details = new StrategicSiteDetails(true, new StrategicSiteName("Test Site"));
        var siteEntity = SiteEntityBuilder.New().Build();

        // when
        var act = async () => await siteEntity.ProvideStrategicSiteDetails(
            details,
            StrategicSiteNameExistMockBuilder.New().WithIsExistAsTrue().BuildObject(),
            CancellationToken.None);

        // when
        await act.Should().ThrowAsync<DomainValidationException>();
    }
}
