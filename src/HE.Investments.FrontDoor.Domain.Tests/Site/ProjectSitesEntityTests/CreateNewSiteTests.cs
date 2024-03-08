using FluentAssertions;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.FrontDoor.Domain.Site;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Site.TestDataBuilders;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Site.ProjectSitesEntityTests;

public class CreateNewSiteTests
{
    [Fact]
    public void ShouldCreateNewSite()
    {
        // given
        var siteName = new SiteName("siteName");
        var projectSites = ProjectSitesEntityBuilder.New().WithSite(new SiteName("siteName2")).Build();

        // when
        var newSite = projectSites.CreateNewSite(siteName);

        // when
        newSite.Name.Should().Be(siteName);
        newSite.Id.IsNew.Should().BeTrue();
    }

    [Theory]
    [InlineData("Existing", "existing")]
    [InlineData("Existing", "Existing")]
    public void ShouldThrowDomainValidationException_WhenNameIsAlreadyOccupied(string existingSiteName, string newSiteNameString)
    {
        // given
        var newSiteName = new SiteName(newSiteNameString);
        var projectSites = ProjectSitesEntityBuilder.New().WithSite(new SiteName(existingSiteName)).Build();

        // when
        var action = () => projectSites.CreateNewSite(newSiteName);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithMessage(ValidationMessages.SiteNameIsAlreadyInUser);
    }
}
