using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.IntegrationTests.Config;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans;

[Order(1)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class StartApplicationIntegrationTests : IntegrationTest
{
    private const string CurrentPageKey = nameof(CurrentPageKey);

    public StartApplicationIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldRedirectToDashboardPage_WhenUserIsLoggedIn()
    {
        // given & when
        var mainPage = await TestClient.NavigateTo(PagesUrls.MainPage);

        // then
        mainPage.UrlEndWith(PagesUrls.DashboardPage);
        SetSharedData(CurrentPageKey, mainPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldRedirectToApplyForALoanPage_WhenStartApplicationButtonIsClicked()
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
    [Order(3)]
    public async Task Order03_ShouldRedirectToAboutLoanPage_WhenStartNowButtonIsClicked()
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
    [Order(4)]
    public async Task Order04_ShouldRedirectToCheckYouDetailsPageAndDisplayMyData_WhenContinueButtonIsClicked()
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
    [Order(5)]
    public async Task Order05_ShouldRedirectToLoanPurpose_WhenContinueButtonIsClicked()
    {
        // given
        var checkYourDetailsPage = GetSharedData<IHtmlDocument>(CurrentPageKey);

        // when
        var continueButton = checkYourDetailsPage.GetGdsSubmitButtonById("continue-button");
        var loanPurpose = await TestClient.SubmitButton(continueButton);

        // then
        loanPurpose
            .UrlEndWith(ApplicationPagesUrls.LoanPurpose)
            .HasTitle("What do you require Homes England funding for?");

        SetSharedData(CurrentPageKey, loanPurpose);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldCreateLoanApplicationWithDraftStatus_WhenContinueButtonIsClicked()
    {
        // given
        var loanPurpose = GetSharedData<IHtmlDocument>(CurrentPageKey);

        // when
        var continueButton = loanPurpose.GetGdsSubmitButtonById("continue-button");
        var taskListPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string>
            {
                { "FundingPurpose", FundingPurpose.BuildingNewHomes.ToString() },
            });

        // then
        taskListPage
            .UrlEndWith(ApplicationPagesUrls.TaskListSuffix)
            .HasTitle("Development loan application")
            .ExtractLastSavedDateFromTaskListPage(out var dateTime);

        // dateTime.Should().BeCloseTo(DateTime.UtcNow.ConvertUtcToUkLocalTime(), TimeSpan.FromMinutes(2));

        var applicationGuid = taskListPage.Url.GetApplicationGuidFromUrl();
        applicationGuid.Should().NotBeEmpty();
        UserData.SetApplicationLoanId(applicationGuid);
    }
}
