using FluentAssertions;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.FrontDoor.Domain.Site;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Site.TestDataBuilders;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Site.ProjectSitesEntityTests;

public class ChangeSiteNameTests
{
    [Fact]
    public void ShouldChangeSiteName_WhenNewSiteNameIsProvided()
    {
        // given
        var siteId = FrontDoorSiteIdTestData.IdTwo;
        var projectSite = ProjectSitesEntityBuilder.New().WithSite(new SiteName("SiteName"), siteId).Build();
        var newSiteName = new SiteName("New Site Name");

        // when
        projectSite.ChangeSiteName(siteId, newSiteName);

        // then
        projectSite.Sites.Single().Name.Should().Be(newSiteName);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenSiteNameIsAlreadyInUse()
    {
        // given
        var siteId = FrontDoorSiteIdTestData.IdTwo;
        var siteNameInUse = new SiteName("SiteNameInUse");
        var projectSite = ProjectSitesEntityBuilder
                                .New()
                                .WithSite(new SiteName("SiteName"), siteId)
                                .WithSite(siteNameInUse)
                                .Build();

        // when
        Action action = () => projectSite.ChangeSiteName(siteId, siteNameInUse);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage(ValidationMessages.SiteNameIsAlreadyInUser);
    }
}
