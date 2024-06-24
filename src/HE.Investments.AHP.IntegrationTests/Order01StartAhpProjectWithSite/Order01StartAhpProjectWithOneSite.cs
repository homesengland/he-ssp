using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Views.Project.Const;
using HE.Investment.AHP.WWW.Views.Site.Const;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.Extensions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.Order01StartAhpProjectWithSite;

[Order(101)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01StartAhpProjectWithOneSite : AhpIntegrationTest
{
    public Order01StartAhpProjectWithOneSite(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_AhpProjectShouldBeCreated()
    {
        // given
        var (projectId, siteId) =
            await InFrontDoor.FrontDoorProjectEligibleForAhpExist(LoginData, ProjectData.GenerateProjectName(), SiteData.GenerateSiteName());
        var currentPage = await TestClient.NavigateTo(ProjectPagesUrl.ProjectStart(UserOrganisationData.OrganisationId, projectId));
        ProjectData.SetProjectId(projectId);
        SiteData.SetSiteId(siteId);

        // when
        var continueButton = currentPage
            .HasTitle(ProjectPageTitles.Start("Affordable Homes Programme 2021-2026 Continuous Market Engagement", "AHP 21-26 CME"))
            .GetSubmitButton("Start now");

        var nextPage = await TestClient.SubmitButton(continueButton);

        // then
        nextPage
            .UrlEndWith(SitePagesUrl.SiteSelect(UserOrganisationData.OrganisationId, ProjectData.ProjectId, true))
            .HasTitle(SitePageTitles.SiteSelect);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldRedirectToSiteStartPage_WhenSelectedSiteIsNotCompleted()
    {
        // given
        (await GetCurrentPage(SitePagesUrl.SiteSelect(UserOrganisationData.OrganisationId, ProjectData.ProjectId, true)))
            .HasNavigationListItem("select-list", out var selectedSiteLink);

        // when
        (await TestClient.NavigateTo(selectedSiteLink))
            .HasTitle(SitePageTitles.SiteConfirmSelect)
            .HasBackLink(out _)
            .HasSubmitButton(out var confirmButton, "Continue");

        var pageAfterConfirmation = await TestClient.SubmitButton(
            confirmButton,
            ("IsConfirmed", true.MapToTrueFalse()));

        // then
        pageAfterConfirmation
            .HasTitle(SitePageTitles.SiteDetails)
            .UrlEndWith(SitePagesUrl.SiteStart(UserOrganisationData.OrganisationId, selectedSiteLink.Href.GetSiteGuidFromUrl()));

        SaveCurrentPage();
    }
}
