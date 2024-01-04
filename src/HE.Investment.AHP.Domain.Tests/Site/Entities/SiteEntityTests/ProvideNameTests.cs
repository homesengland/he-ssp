using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;
using HE.Investments.Common.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Site.Entities.SiteEntityTests;

public class ProvideNameTests
{
    [Fact]
    public async Task ShouldSaveSiteName_WhenSiteNameIsProvided()
    {
        // given
        var siteName = new SiteName("Test Site");
        var siteEntity = SiteEntityBuilder.New().Build();

        // when
        await siteEntity.ProvideName(siteName, SiteNameExistMockBuilder.New().WithIsExistAsFalse().BuildObject(), CancellationToken.None);

        // when
        siteEntity.Name.Should().Be(siteName);
    }

    [Fact]
    public async Task ShouldThrowException_WhenSiteNameIsAlreadyExist()
    {
        // given
        var siteName = new SiteName("Test Site");
        var siteEntity = SiteEntityBuilder.New().Build();

        // when
        var act = async () => await siteEntity.ProvideName(siteName, SiteNameExistMockBuilder.New().WithIsExistAsTrue().BuildObject(), CancellationToken.None);

        // when
        await act.Should().ThrowAsync<DomainValidationException>();
    }
}
