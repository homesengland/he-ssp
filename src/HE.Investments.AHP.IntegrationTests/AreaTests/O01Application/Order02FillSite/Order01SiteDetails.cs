using System.Diagnostics.CodeAnalysis;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Pages;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.Common.Contract;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order02FillSite;

[Order(201)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01SiteDetails : AhpIntegrationTest
{
    public Order01SiteDetails(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldOpenProjectSiteList()
    {
        // given
        var projectDetails = await TestClient.NavigateTo(ProjectPagesUrl.ProjectDetails(UserOrganisationData.OrganisationId, ProjectData.ProjectId));

        // when
        var viewSitesButton = projectDetails.GetLinkButton("View");
        var listOfSitesPage = await TestClient.NavigateTo(viewSitesButton);

        // then
        listOfSitesPage
            .UrlEndWith(ProjectPagesUrl.ProjectSiteList(UserOrganisationData.OrganisationId, ShortGuid.FromString(ProjectData.ProjectId).Value))
            .HasTitle("Sites")
            .HasBackLink(out _);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldOpenProjectSiteDetails()
    {
        // given
        var listOfSitesPage = await GetCurrentPage(ProjectPagesUrl.ProjectSiteList(UserOrganisationData.OrganisationId, ProjectData.ProjectId));

        // when
        listOfSitesPage.HasLinkWithText(SiteData.SiteName, out var linkToSite);
        SiteData.SetSiteId(linkToSite.Href.GetSiteGuidFromUrl());
        var siteDetails = await TestClient.NavigateTo(linkToSite);

        // then
        siteDetails.HasTitle(SiteData.SiteName);

        SaveCurrentPage();
    }
}
