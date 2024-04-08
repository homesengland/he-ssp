using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Views.Application;
using HE.Investment.AHP.WWW.Views.Site.Const;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.FillApplication;

[Order(1)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01StartAhpApplication : AhpIntegrationTest
{
    public Order01StartAhpApplication(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldRedirectToApplicationList_WhenUserIsLoggedIn()
    {
        // given & when
        var mainPage = await TestClient.NavigateTo(MainPagesUrl.MainPage);

        // then
        mainPage
            .UrlEndWith(MainPagesUrl.ApplicationList)
            .HasTitle(ApplicationPageTitles.ApplicationList);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldNavigateToApplicationLandingPage()
    {
        // given
        var startButton = (await GetCurrentPage(MainPagesUrl.ApplicationList)).GetLinkButton("Start");

        // when
        var applicationNamePage = await TestClient.NavigateTo(startButton);

        // then
        applicationNamePage
            .UrlEndWith(ApplicationPagesUrl.Start)
            .HasTitle(ApplicationPageTitles.Start);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldNavigateToSiteSelectPage()
    {
        // given
        var startButton = (await GetCurrentPage(ApplicationPagesUrl.Start)).GetStartButton();

        // when
        var siteSelectPage = await TestClient.SubmitButton(startButton);

        // then
        siteSelectPage
            .UrlEndWith(SitePagesUrl.SiteSelect)
            .HasTitle(SitePageTitles.SiteSelect);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldNavigateToSiteConfirmPage()
    {
        // given
        var siteSelectPage = await GetCurrentPage(SitePagesUrl.SiteSelect);
        siteSelectPage.HasNavigationListItem("select-list", out var selectSiteLink);

        // when
        var siteConfirmPage = await TestClient.NavigateTo(selectSiteLink);

        // then
        ApplicationData.SetSiteId(siteConfirmPage.Url.GetSiteGuidFromUrl());
        siteConfirmPage
            .UrlEndWith(SitePagesUrl.SiteConfirm(ApplicationData.SiteId))
            .HasTitle(SitePageTitles.SiteConfirmSelect);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldProvideSiteConfirmation()
    {
        // given
        var siteConfirmPage = await GetCurrentPage(SitePagesUrl.SiteConfirm(ApplicationData.SiteId));
        siteConfirmPage
            .UrlEndWith(SitePagesUrl.SiteConfirm(ApplicationData.SiteId))
            .HasTitle(SitePageTitles.SiteConfirmSelect)
            .HasBackLink(out _)
            .HasContinueButton(out var continueButton);

        // when
        var applicationNamePage = await TestClient.SubmitButton(continueButton, ("IsConfirmed", "True"));

        // then
        applicationNamePage
            .UrlEndWith(ApplicationPagesUrl.ApplicationName(ApplicationData.SiteId))
            .HasTitle(ApplicationPageTitles.ApplicationName);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldProvideApplicationNameAndNavigateToTenurePage()
    {
        // given
        var applicationData = ApplicationData.GenerateApplicationName();
        var applicationNamePage = await GetCurrentPage(ApplicationPagesUrl.ApplicationName(ApplicationData.SiteId));
        applicationNamePage
            .UrlEndWith(ApplicationPagesUrl.ApplicationName(ApplicationData.SiteId))
            .HasTitle(ApplicationPageTitles.ApplicationName)
            .HasBackLink(out _)
            .HasContinueButton(out var continueButton);

        // when
        var applicationTenurePage = await TestClient.SubmitButton(continueButton, ("Name", applicationData.ApplicationName));

        // then
        applicationTenurePage
            .UrlWithoutQueryEndsWith(ApplicationPagesUrl.Tenure(applicationData.SiteId))
            .HasTitle(ApplicationPageTitles.Tenure);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ShouldProvideTenureAndNavigateToApplicationTaskList()
    {
        // given & when & then
        var taskListPage = await TestQuestionPage(
            ApplicationPagesUrl.Tenure(ApplicationData.SiteId),
            ApplicationPageTitles.Tenure,
            ApplicationPagesUrl.TaskListSuffix,
            ("Tenure", ApplicationData.Tenure.ToString()));

        ApplicationData.SetApplicationId(taskListPage.Url.GetApplicationGuidFromUrl());
    }
}
