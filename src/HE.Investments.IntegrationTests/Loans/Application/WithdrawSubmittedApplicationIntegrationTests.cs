using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Assertions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.Application;

[Order(4)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class WithdrawSubmittedApplicationIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;
    private readonly string _withdrawnInsetText = "Your application has been withdrawn, contact your Transaction Manager if you need to discuss a change";

    public WithdrawSubmittedApplicationIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldOpenWithdrawPage_WhenApplicationIsInApplicationSubmittedState()
    {
        // given
        var applicationDashboardPage = await TestClient.NavigateTo(ApplicationPagesUrls.ApplicationDashboard(_applicationLoanId));

        // when
        var withdrawApplicationButton = applicationDashboardPage.GetAnchorElementById("withdraw-application");
        var withdrawPage = await TestClient.NavigateTo(withdrawApplicationButton);

        // then
        withdrawPage
            .UrlEndWith(ApplicationPagesUrls.WithdrawSuffix)
            .HasLabelTitle("Why are you withdrawing your application")
            .HasGdsSubmitButton("continue-button", out _);

        SetSharedData(SharedKeys.CurrentPageKey, withdrawPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldDisplayValidationError_WhenWithdrawReasonIsNotProvided()
    {
        // given
        var withdrawPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var acceptAndSubmitButton = withdrawPage.GetGdsSubmitButtonById("continue-button");

        // when
        withdrawPage = await TestClient.SubmitButton(
            acceptAndSubmitButton,
            new Dictionary<string, string> { { "WithdrawReason", string.Empty } });

        // then
        withdrawPage
            .UrlEndWith(ApplicationPagesUrls.WithdrawSuffix)
            .HasLabelTitle("Why are you withdrawing your application")
            .ContainsValidationMessage(ValidationErrorMessage.EnterWhyYouWantToWithdrawApplication);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldDisplayValidationError_WhenWithdrawReasonIsLongerThan1500Characters()
    {
        // given
        var withdrawPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var acceptAndSubmitButton = withdrawPage.GetGdsSubmitButtonById("continue-button");

        // when
        withdrawPage = await TestClient.SubmitButton(
            acceptAndSubmitButton,
            new Dictionary<string, string> { { "WithdrawReason", TextTestData.TextThatExceedsLongInputLimit } });

        // then
        withdrawPage
            .UrlEndWith(ApplicationPagesUrls.WithdrawSuffix)
            .HasLabelTitle("Why are you withdrawing your application")
            .ContainsValidationMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.WithdrawReason));
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldMoveToApplicationDashboard_WhenWithdrawReasonIsProvided()
    {
        // given
        var withdrawPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var acceptAndSubmitButton = withdrawPage.GetGdsSubmitButtonById("continue-button");

        // when
        var applicationDashboardPage = await TestClient.SubmitButton(
            acceptAndSubmitButton,
            new Dictionary<string, string> { { "WithdrawReason", "very important reason" } });

        // then
        applicationDashboardPage
            .UrlEndWith(ApplicationPagesUrls.ApplicationDashboard(_applicationLoanId))
            .HasSuccessNotificationBanner("project has been withdrawn")
            .HasNotEmptyTitle();

        SetSharedData(SharedKeys.CurrentPageKey, applicationDashboardPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(5)]
    public void Order05_ShouldNotSeeWithdrawButton_WhenApplicationWasAlreadyWithdrawn()
    {
        // given
        var applicationDashboardPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);

        // when
        var withdrawApplicationButton = applicationDashboardPage.GetAnchorElementById("withdraw-application");

        // then
        withdrawApplicationButton.Should().BeNull();
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldSeeCompanyStructureInReadOnlyMode_WhenApplicationIsInWithdrawnState()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrls.TaskList(_applicationLoanId));
        var linkToCompanyStructureSection = taskListPage.GetAnchorElementById("company-structure-section-link");

        // when
        var checkAnswersPage = await TestClient.NavigateTo(linkToCompanyStructureSection);

        // then
        checkAnswersPage
            .UrlEndWith(CompanyStructurePagesUrls.CheckYourAnswersSuffix)
            .HasInsetText(_withdrawnInsetText);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ShouldSeeFundingInReadOnlyMode_WhenApplicationIsInWithdrawnState()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrls.TaskList(_applicationLoanId));
        var linkToFundingSection = taskListPage.GetAnchorElementById("funding-section-link");

        // when
        var checkAnswersPage = await TestClient.NavigateTo(linkToFundingSection);

        // then
        checkAnswersPage
            .UrlEndWith(FundingPageUrls.CheckYourAnswersSuffix)
            .HasInsetText(_withdrawnInsetText);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ShouldSeeSecurityInReadOnlyMode_WhenApplicationIsInWithdrawnState()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrls.TaskList(_applicationLoanId));
        var linkToSecuritySection = taskListPage.GetAnchorElementById("security-section-link");

        // when
        var checkAnswersPage = await TestClient.NavigateTo(linkToSecuritySection);

        // when && then
        checkAnswersPage
            .UrlEndWith(SecurityPageUrls.CheckYourAnswersSuffix)
            .HasInsetText(_withdrawnInsetText);
    }
}
