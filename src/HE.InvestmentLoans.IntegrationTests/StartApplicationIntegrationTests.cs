using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.IntegrationTests.Config;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.LoansHelpers;
using HE.InvestmentLoans.IntegrationTests.LoansHelpers.Extensions;
using HE.InvestmentLoans.IntegrationTests.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests;

[Order(1)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class StartApplicationIntegrationTests : IntegrationTest
{
    private const string? Skip = "Waits for DevOps configuration - #76791";

    private const string CurrentPageKey = nameof(CurrentPageKey);

    private const string NewApplicationKey = nameof(NewApplicationKey);

    private readonly UserConfig _userConfig;

    public StartApplicationIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _userConfig = new UserConfig();
    }

    [Fact(Skip = Skip)]
    [Order(1)]
    public async Task Order01_ShouldRedirectToDashboardPage_WhenUserIsLoggedIn()
    {
        // given & when
        var mainPage = await TestClient.NavigateTo(PagesUrls.MainPage);

        // then
        mainPage.Url.Should().EndWith(PagesUrls.DashboardPage);
        SetSharedData(CurrentPageKey, mainPage);
    }

    [Fact(Skip = Skip)]
    [Order(2)]
    public async Task Order02_ShouldRedirectToApplyForALoanPage_WhenStartApplicationButtonIsClicked()
    {
        // given
        var currentPage = GetSharedData<IHtmlDocument>(CurrentPageKey);

        // when
        var startApplicationLink = currentPage.GetGdsLinkButtonById("start-application-link");
        var applyForLoanPage = await TestClient.ClickAHrefElement(startApplicationLink);

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
        var currentPage = GetSharedData<IHtmlDocument>(CurrentPageKey);

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
        var currentPage = GetSharedData<IHtmlDocument>(CurrentPageKey);

        // when
        var continueButton = currentPage.GetGdsSubmitButtonById("continue-button");
        var checkYourDetailsPage = await TestClient.SubmitButton(continueButton);

        // then
        checkYourDetailsPage.Url.Should().EndWith(ApplicationPagesUrls.CheckYourDetails);
        checkYourDetailsPage.GetPageTitle().Should().Be("Check your details");
        var items = checkYourDetailsPage.GetSummaryListItems();

        items[CheckYourDetailsFields.RegisteredCompanyName].Should().Be(_userConfig.OrganizationName);
        items[CheckYourDetailsFields.CompanyRegistrationNumber].Should().Be(_userConfig.OrganizationRegistrationNumber);
        items[CheckYourDetailsFields.CompanyAddress].Should().Be(_userConfig.OrganizationAddress);
        items[CheckYourDetailsFields.ContactName].Should().Be(_userConfig.ContactName);
        items[CheckYourDetailsFields.EmailAddress].Should().Be(_userConfig.Email);
        items[CheckYourDetailsFields.TelephoneNumber].Should().Be(_userConfig.TelephoneNumer);
        SetSharedData(CurrentPageKey, checkYourDetailsPage);
    }

    [Fact(Skip = Skip)]
    [Order(5)]
    public async Task Order05_ShouldRedirectToLoanPurpose_WhenContinueButtonIsClicked()
    {
        // given
        var currentPage = GetSharedData<IHtmlDocument>(CurrentPageKey);

        // when
        var continueButton = currentPage.GetGdsSubmitButtonById("continue-button");
        var loanPurpose = await TestClient.SubmitButton(continueButton);

        // then
        loanPurpose.Url.Should().EndWith(ApplicationPagesUrls.LoanPurpose);
        loanPurpose.GetPageTitle().Should().Be("What do you require Homes England funding for?");
        SetSharedData(CurrentPageKey, loanPurpose);
    }

    [Fact(Skip = Skip)]
    [Order(6)]
    public async Task Order06_ShouldCreateLoanApplicationWithDraftStatus_WhenContinueButtonIsClicked()
    {
        // given
        var currentPage = await TestClient.NavigateTo(ApplicationPagesUrls.LoanPurpose);

        // when
        var continueButton = currentPage.GetGdsSubmitButtonById("continue-button");
        var taskListPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string>
            {
                { "FundingPurpose", FundingPurpose.BuildingNewHomes.ToString() },
            });

        // then
        taskListPage.Url.Should().EndWith(ApplicationPagesUrls.TaskList);
        taskListPage.GetPageTitle().Should().Be("Development loan application");
        var applicationGuid = taskListPage.Url.GetApplicationGuidFromUrl();
        applicationGuid.Should().NotBeEmpty();
        SetSharedData(CurrentPageKey, taskListPage);
        SetSharedData(NewApplicationKey, applicationGuid);
    }
}
