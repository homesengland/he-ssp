using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investments.Common.Extensions;
using HE.Investments.IntegrationTestsFramework.Assertions;
using HE.Investments.Loans.Contract.Application.Enums;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW.Views.LoanApplicationV2.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order01StartApplication;

[Order(1)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01StartApplicationIntegrationTests : IntegrationTest
{
    public Order01StartApplicationIntegrationTests(LoansIntegrationTestFixture fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldRedirectToUserDashboardPage_WhenUserIsLoggedIn()
    {
        // given & when
        var mainPage = await TestClient.NavigateTo(PagesUrls.MainPage);

        // then
        mainPage.UrlEndWith(PagesUrls.DashboardPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldRedirectToDashboardPage()
    {
        // given && when
        var dashboardPage = await TestClient.NavigateTo(PagesUrls.DashboardPage);

        // then
        dashboardPage.UrlEndWith(PagesUrls.DashboardPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldRedirectToLoanApplyInformation_WhenContinueButtonIsClicked()
    {
        // given
        await FrontDoorProjectCrmRepository.CreateProject(UserData.GenerateProjectPrefillData(), LoginData);

        var currentPage = await TestClient.NavigateTo(ApplicationPagesUrls.AboutLoanPage(UserData.ProjectPrefillData.Id));
        var continueButton = currentPage
            .UrlEndWith(ApplicationPagesUrls.AboutLoanPage(UserData.ProjectPrefillData.Id))
            .HasTitle(LoanApplicationPageTitles.AboutLoan)
            .GetGdsSubmitButtonById("continue-button");

        // when
        var nextPage = await TestClient.SubmitButton(continueButton, ("InformationAgreement", "true"));

        // then
        nextPage
            .UrlEndWith(ApplicationPagesUrls.LoanApplyInformation(UserData.ProjectPrefillData.Id))
            .HasTitle(LoanApplicationPageTitles.LoanApplyInformation);
        SaveCurrentPage();
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldRedirectToCheckYouDetailsPageAndDisplayMyData_WhenContinueButtonIsClicked()
    {
        // given
        var currentPage = await GetCurrentPage(ApplicationPagesUrls.LoanApplyInformation(UserData.ProjectPrefillData.Id));
        var continueButton = currentPage
            .UrlEndWith(ApplicationPagesUrls.LoanApplyInformation(UserData.ProjectPrefillData.Id))
            .HasTitle(LoanApplicationPageTitles.LoanApplyInformation)
            .GetGdsSubmitButtonById("continue-button");

        // when
        var nextPage = await TestClient.SubmitButton(continueButton);

        // then
        var summary = nextPage
            .UrlEndWith(ApplicationPagesUrls.CheckYourDetails(UserData.ProjectPrefillData.Id))
            .HasTitle("Check your details")
            .GetSummaryListItems();

        summary.Should().ContainKey(CheckYourDetailsFields.RegisteredCompanyName).WhoseValue.Value.Should().NotBeNullOrWhiteSpace();
        summary.Should().ContainKey(CheckYourDetailsFields.CompanyAddress).WhoseValue.Value.Should().NotBeNullOrWhiteSpace();
        summary.Should().ContainKey(CheckYourDetailsFields.ContactName).WhoseValue.Value.Should().NotBeNullOrWhiteSpace();
        summary.Should().ContainKey(CheckYourDetailsFields.EmailAddress).WithValue(LoginData.Email);
        summary.Should().ContainKey(CheckYourDetailsFields.TelephoneNumber).WhoseValue.Value.Should().NotBeNullOrWhiteSpace();
        SaveCurrentPage();
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldRedirectToLoanPurpose_WhenContinueButtonIsClicked()
    {
        // given
        var currentPage = await GetCurrentPage(ApplicationPagesUrls.CheckYourDetails(UserData.ProjectPrefillData.Id));
        var continueButton = currentPage
            .UrlEndWith(ApplicationPagesUrls.CheckYourDetails(UserData.ProjectPrefillData.Id))
            .HasTitle("Check your details")
            .GetGdsSubmitButtonById("continue-button");

        // when
        var nextPage = await TestClient.SubmitButton(continueButton);

        // then
        nextPage
            .UrlEndWith(ApplicationPagesUrls.LoanPurpose(UserData.ProjectPrefillData.Id))
            .HasTitle("What do you require Homes England funding for?")
            .HasRadio("FundingPurpose", value: FundingPurpose.BuildingNewHomes.ToString());
        SaveCurrentPage();
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldRedirectToApplicationName_WhenContinueButtonIsClicked()
    {
        // given
        var currentPage = await GetCurrentPage(ApplicationPagesUrls.LoanPurpose(UserData.ProjectPrefillData.Id));
        var continueButton = currentPage
            .UrlEndWith(ApplicationPagesUrls.LoanPurpose(UserData.ProjectPrefillData.Id))
            .HasTitle("What do you require Homes England funding for?")
            .HasRadio("FundingPurpose", value: FundingPurpose.BuildingNewHomes.ToString())
            .GetGdsSubmitButtonById("continue-button");

        // when
        var nextPage = await TestClient.SubmitButton(
            continueButton,
            ("FundingPurpose", FundingPurpose.BuildingNewHomes.ToString()));

        // then
        nextPage
            .UrlEndWith(ApplicationPagesUrls.ApplicationName(UserData.ProjectPrefillData.Id))
            .HasTitle("Name your application")
            .HasInput("LoanApplicationName", value: UserData.ProjectPrefillData.Name);
        SaveCurrentPage();
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ShouldCreateLoanApplicationWithDraftStatus_WhenContinueButtonIsClicked()
    {
        // given
        var currentPage = await GetCurrentPage(ApplicationPagesUrls.ApplicationName(UserData.ProjectPrefillData.Id));
        var continueButton = currentPage
            .UrlEndWith(ApplicationPagesUrls.ApplicationName(UserData.ProjectPrefillData.Id))
            .HasTitle("Name your application")
            .GetGdsSubmitButtonById("continue-button");
        var applicationName = UserData.GenerateApplicationName();

        // when
        var nextPage = await TestClient.SubmitButton(continueButton, ("LoanApplicationName", applicationName));

        // then
        nextPage
            .UrlEndWith(ApplicationPagesUrls.TaskListSuffix)
            .HasTitle(UserData.LoanApplicationName)
            .ExtractLastSavedDateFromTaskListPage(out var dateTime);

        dateTime.Should().BeCloseTo(DateTime.UtcNow.ConvertUtcToUkLocalTime(), TimeSpan.FromMinutes(2));

        var applicationGuid = nextPage.Url.GetApplicationGuidFromUrl();
        applicationGuid.Should().NotBeEmpty();
        UserData.SetApplicationLoanId(applicationGuid);
    }
}
