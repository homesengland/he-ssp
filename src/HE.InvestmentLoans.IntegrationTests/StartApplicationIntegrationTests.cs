using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests;

[Order(0)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class StartApplicationIntegrationTests : IntegrationTest
{
    private const string? Skip = null;

    private const string CurrentPageKey = "CurrentPage";

    public StartApplicationIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = Skip)]
    [Order(1)]
    public async Task Order01_ShouldRedirectToDashboardPage_WhenUserIsLoggedIn()
    {
        // given & when
        var mainPage = await TestClient.AsLoggedUser().NavigateTo(PagesUrls.MainPage);

        // then
        mainPage.Url.Should().EndWith(PagesUrls.DashboardPage);
        SetSharedData(CurrentPageKey, mainPage);
    }

    [Fact(Skip = Skip)]
    [Order(2)]
    public async Task Order02_ShouldRedirectToApplyForALoanPage_WhenStartApplicationButtonIsClicked()
    {
        // given
        var currentPage = await TestClient.AsLoggedUser().NavigateTo(PagesUrls.DashboardPage);

        // when
        var startApplicationLink = currentPage.GetGdsLinkButtonById("start-application-link");
        var applyForLoanPage = await TestClient.ClickAhref(startApplicationLink);

        // then
        applyForLoanPage.Url.Should().EndWith(ApplicationPagesUrls.StartPage);
        applyForLoanPage.GetPageTitle().Should().Be("Apply for a development loan");
        SetSharedData(CurrentPageKey, applyForLoanPage);
    }

    [Fact(Skip = Skip)]
    [Order(3)]
    public async Task Order03_ShouldRedirectToAboutLoanPage_WhenStartNowButtonIsClicked()
    {
        // given
        var currentPage =await TestClient.AsLoggedUser().NavigateTo(ApplicationPagesUrls.StartPage);

        // when
        var startNowButton = currentPage.GetGdsSubmitButtonById("start-now-button");
        var aboutLoanPage = await TestClient.SubmitButton(startNowButton);

        // then
        aboutLoanPage.Url.Should().EndWith(ApplicationPagesUrls.AboutLoanPage);
        aboutLoanPage.GetPageTitle().Should().Be("What you need to know about the loan");
        SetSharedData(CurrentPageKey, aboutLoanPage);
    }

    [Fact(Skip = Skip)]
    [Order(4)]
    public async Task Order04_ShouldRedirectToCheckYouDetailsPageAndDisplayMyData_WhenContinueButtonIsClicked()
    {
        // given
        var currentPage = await TestClient.AsLoggedUser().NavigateTo(ApplicationPagesUrls.AboutLoanPage);

        // when
        var continueButton = currentPage.GetGdsSubmitButtonById("continue-button");
        var checkYourDetailsPage = await TestClient.SubmitButton(continueButton);

        // then
        checkYourDetailsPage.Url.Should().EndWith(ApplicationPagesUrls.CheckYourDetails);
        checkYourDetailsPage.GetPageTitle().Should().Be("Check your details");
        SetSharedData(CurrentPageKey, checkYourDetailsPage);
    }
}
