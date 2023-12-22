using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Views.Application;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.IntegrationTestsFramework.Config;
using HE.Investments.IntegrationTestsFramework.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.Application;

[Order(1)]
public class Order01StartAhpApplication : AhpIntegrationTest
{
    public Order01StartAhpApplication(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact]
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

    [Fact]
    [Order(2)]
    public async Task Order02_ShouldNavigateToApplicationNamePage()
    {
        // given
        var applicationListPage = await GetCurrentPage(MainPagesUrl.ApplicationList);
        applicationListPage.HasGdsLinkButton("start-application-link", out var startApplicationLink);

        // when
        var applicationNamePage = await TestClient.NavigateTo(startApplicationLink);

        // then
        applicationNamePage
            .UrlEndWith(ApplicationPagesUrl.ApplicationName)
            .HasTitle(ApplicationPageTitles.ApplicationName);

        SaveCurrentPage();
    }

    [Fact]
    [Order(3)]
    public async Task Order03_ShouldProvideApplicationNameAndNavigateToTenurePage()
    {
        // given
        var applicationNamePage = await GetCurrentPage(ApplicationPagesUrl.ApplicationName);
        applicationNamePage.HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var tenurePage = await TestClient.SubmitButton(
            continueButton,
            new Dictionary<string, string> { { nameof(ApplicationBasicModel.Name), "Test Application" } });

        // then
        tenurePage
            .UrlWithoutQueryEndsWith(ApplicationPagesUrl.Tenure)
            .HasTitle(ApplicationPageTitles.Tenure);

        SaveCurrentPage();
    }
}
