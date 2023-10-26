using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.Application.Order01StartApplication;

[Order(1)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01StartApplicationIntegrationTests : IntegrationTest
{
    private const string CurrentPageKey = nameof(CurrentPageKey);

    public Order01StartApplicationIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldRedirectToUserOrganisationPage_WhenUserIsLoggedIn()
    {
        // given & when
        var mainPage = await TestClient.NavigateTo(PagesUrls.MainPage);

        // then
        mainPage.UrlEndWith(UserOrganisationPagesUrls.UserOrganisation);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldRedirectToDashboardPage()
    {
        // given && when
        var dashboardPage = await TestClient.NavigateTo(PagesUrls.DashboardPage);

        // then
        dashboardPage.UrlEndWith(PagesUrls.DashboardPage);
        SetSharedData(CurrentPageKey, dashboardPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldRedirectToApplyForALoanPage_WhenStartApplicationButtonIsClicked()
    {
        // given
        var dashboardPage = GetSharedData<IHtmlDocument>(CurrentPageKey);

        // when
        var startApplicationLink = dashboardPage.GetGdsLinkButtonById("start-application-link");
        var applyForLoanPage = await TestClient.NavigateTo(startApplicationLink);

        // then
        applyForLoanPage
            .UrlEndWith(ApplicationPagesUrls.StartPage)
            .HasTitle("Apply for a development loan");

        SetSharedData(CurrentPageKey, applyForLoanPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldRedirectToAboutLoanPage_WhenStartNowButtonIsClicked()
    {
        // given
        var applyForLoanPage = GetSharedData<IHtmlDocument>(CurrentPageKey);

        // when
        var startNowButton = applyForLoanPage.GetGdsSubmitButtonById("start-now-button");
        var aboutLoanPage = await TestClient.SubmitButton(startNowButton);

        // then
        aboutLoanPage
            .UrlEndWith(ApplicationPagesUrls.AboutLoanPage)
            .HasTitle("What you need to know about the loan");

        SetSharedData(CurrentPageKey, aboutLoanPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldRedirectToCheckYouDetailsPageAndDisplayMyData_WhenContinueButtonIsClicked()
    {
        // given
        var aboutLoanPage = GetSharedData<IHtmlDocument>(CurrentPageKey);

        // when
        var continueButton = aboutLoanPage.GetGdsSubmitButtonById("continue-button");
        var checkYourDetailsPage = await TestClient.SubmitButton(continueButton);

        // then
        var items = checkYourDetailsPage
            .UrlEndWith(ApplicationPagesUrls.CheckYourDetails)
            .HasTitle("Check your details")
            .GetSummaryListItems();

        items[CheckYourDetailsFields.RegisteredCompanyName].Should().Be(UserData.OrganizationName);
        items[CheckYourDetailsFields.CompanyRegistrationNumber].Should().Be(UserData.OrganizationRegistrationNumber);
        items[CheckYourDetailsFields.CompanyAddress].Should().Be(UserData.OrganizationAddress);
        items[CheckYourDetailsFields.ContactName].Should().Be(UserData.ContactName);
        items[CheckYourDetailsFields.EmailAddress].Should().Be(UserData.Email);
        items[CheckYourDetailsFields.TelephoneNumber].Should().Be(UserData.TelephoneNumber.ToString());
        SetSharedData(CurrentPageKey, checkYourDetailsPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldRedirectToLoanPurpose_WhenContinueButtonIsClicked()
    {
        // given
        var checkYourDetailsPage = GetSharedData<IHtmlDocument>(CurrentPageKey);

        // when
        var continueButton = checkYourDetailsPage.GetGdsSubmitButtonById("continue-button");
        var loanPurposePage = await TestClient.SubmitButton(continueButton);

        // then
        loanPurposePage
            .UrlEndWith(ApplicationPagesUrls.LoanPurpose)
            .HasTitle("What do you require Homes England funding for?");

        SetSharedData(CurrentPageKey, loanPurposePage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ShouldRedirectToApplicationName_WhenContinueButtonIsClicked()
    {
        // given
        var loanPurposePage = GetSharedData<IHtmlDocument>(CurrentPageKey);

        // when
        var continueButton = loanPurposePage.GetGdsSubmitButtonById("continue-button");
        var applicationNamePage = await TestClient.SubmitButton(
            continueButton,
            new Dictionary<string, string> { { "FundingPurpose", FundingPurpose.BuildingNewHomes.ToString() }, });

        // then
        applicationNamePage
            .UrlEndWith(ApplicationPagesUrls.ApplicationName)
            .HasTitle("Name your application");

        SetSharedData(CurrentPageKey, applicationNamePage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ShouldCreateLoanApplicationWithDraftStatus_WhenContinueButtonIsClicked()
    {
        // given
        var applicationNamePage = GetSharedData<IHtmlDocument>(CurrentPageKey);

        // when
        var continueButton = applicationNamePage.GetGdsSubmitButtonById("continue-button");
        var taskListPage = await TestClient.SubmitButton(
            continueButton,
            new Dictionary<string, string> { { "LoanApplicationName", $"Application-{Guid.NewGuid()}" }, });

        // then
        taskListPage
            .UrlEndWith(ApplicationPagesUrls.TaskListSuffix)
            .HasTitle("Development loan application")
            .ExtractLastSavedDateFromTaskListPage(out var dateTime);

        dateTime.Should().BeCloseTo(DateTime.UtcNow.ConvertUtcToUkLocalTime(), TimeSpan.FromMinutes(2));

        var applicationGuid = taskListPage.Url.GetApplicationGuidFromUrl();
        applicationGuid.Should().NotBeEmpty();
        UserData.SetApplicationLoanId(applicationGuid);
    }
}
