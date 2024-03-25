using FluentAssertions;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Site.TestDataBuilders;
using HE.Investments.FrontDoor.Shared.Project;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Site.ProjectSitesEntityTests;

public class AreSitesValidForLoanApplicationTests
{
    [Fact]
    public void ShouldReturnFalse_WhenSitesAreNotValidForLoanApplication()
    {
        // given
        var siteOne = ProjectSiteEntityBuilder
            .New(new SiteName("first test name"), new FrontDoorProjectId("first project id"), new FrontDoorSiteId("first site id"))
            .WithPlanningStatus(SitePlanningStatus.DetailedPlanningApplicationSubmitted)
            .WithHomesNumber(150)
            .WithLocalAuthority("E08000012", "Liverpool")
            .Build();

        var siteTwo = ProjectSiteEntityBuilder
            .New(new SiteName("second test name"), new FrontDoorProjectId("second project id"), new FrontDoorSiteId("second site id"))
            .WithPlanningStatus(SitePlanningStatus.DetailedPlanningApplicationSubmitted)
            .WithHomesNumber(150)
            .WithLocalAuthority("E08000012", "Liverpool")
            .Build();

        var projectSites = ProjectSitesEntityBuilder
            .New()
            .AddSite(siteOne)
            .AddSite(siteTwo)
            .Build();

        // when
        var result = projectSites.AreSitesValidForLoanApplication();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnTrue_WhenSitesAreValidForLoanApplication()
    {
        // given
        var site = ProjectSiteEntityBuilder
            .New(null, null, null)
            .WithPlanningStatus(SitePlanningStatus.DetailedPlanningApplicationSubmitted)
            .WithHomesNumber(150)
            .WithLocalAuthority("E08000012", "Liverpool")
            .Build();

        var projectSites = ProjectSitesEntityBuilder
            .New()
            .AddSite(site)
            .Build();

        // when
        var result = projectSites.AreSitesValidForLoanApplication();

        // then
        result.Should().BeTrue();
    }
}
