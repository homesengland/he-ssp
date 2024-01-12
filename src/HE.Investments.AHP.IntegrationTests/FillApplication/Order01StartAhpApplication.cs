using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Views.Application;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.FillApplication;

[Order(1)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01StartAhpApplication : AhpIntegrationTest
{
    public Order01StartAhpApplication(IntegrationTestFixture<Program> fixture)
        : base(fixture)
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
        var applicationListPage = await GetCurrentPage(MainPagesUrl.ApplicationList);
        applicationListPage.HasGdsLinkButton("start-application-link", out var startApplicationLink);

        // when
        var applicationNamePage = await TestClient.NavigateTo(startApplicationLink);

        // then
        applicationNamePage
            .UrlEndWith(ApplicationPagesUrl.StartName)
            .HasTitle(ApplicationPageTitles.Start);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldNavigateToApplicationNamePage()
    {
        // given
        var applicationListPage = await GetCurrentPage(ApplicationPagesUrl.StartName);
        applicationListPage.HasGdsLinkButton("continue-button", out var startApplicationLink);

        // when
        var applicationNamePage = await TestClient.NavigateTo(startApplicationLink);

        // then
        applicationNamePage
            .UrlEndWith(ApplicationPagesUrl.ApplicationName)
            .HasTitle(ApplicationPageTitles.ApplicationName);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldProvideApplicationNameAndNavigateToTenurePage()
    {
        // given
        var applicationNamePage = await GetCurrentPage(ApplicationPagesUrl.ApplicationName);
        applicationNamePage.HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var tenurePage = await TestClient.SubmitButton(
            continueButton,
            new Dictionary<string, string> { { nameof(ApplicationBasicModel.Name), ApplicationData.GenerateApplicationName() } });

        // then
        tenurePage
            .UrlWithoutQueryEndsWith(ApplicationPagesUrl.Tenure)
            .HasTitle(ApplicationPageTitles.Tenure);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order5_ShouldProvideTenureAndNavigateToApplicationTaskList()
    {
        // given
        var tenurePage = await GetCurrentPage(ApplicationPagesUrl.Tenure);
        tenurePage.HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var taskListPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(ApplicationBasicModel.Tenure), "AffordableRent"));

        // then
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasValidApplicationReferenceNumber();

        ApplicationData.SetApplicationId(taskListPage.Url.GetApplicationGuidFromUrl());

        SaveCurrentPage();
    }
}
