using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.InvestmentLoans.WWW.Views.LoanApplicationV2.Const;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.Application.Order04Withdraw;

[Order(3)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class WithdrawDraftApplicationIntegrationTests : IntegrationTest
{
    public WithdrawDraftApplicationIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldOpenWithdrawPage_WhenApplicationIsInDraftState()
    {
        // given
        var applicationToWithdrawId = await CreateNewApplicationDraft();

        var applicationDashboardPage = await TestClient.NavigateTo(ApplicationPagesUrls.ApplicationDashboard(applicationToWithdrawId));

        // when
        var withdrawApplicationButton = applicationDashboardPage.GetAnchorElementById("withdraw-application");
        var withdrawPage = await TestClient.NavigateTo(withdrawApplicationButton);

        // then
        withdrawPage
            .UrlEndWith(ApplicationPagesUrls.WithdrawSuffix)
            .HasLabelTitle(LoanApplicationPageTitles.Withdraw)
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
            .HasLabelTitle(LoanApplicationPageTitles.Withdraw)
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
            .HasLabelTitle(LoanApplicationPageTitles.Withdraw)
            .ContainsValidationMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.WithdrawReason));
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldMoveToMainDashboard_WhenWithdrawReasonIsProvided()
    {
        // given
        var withdrawPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var acceptAndSubmitButton = withdrawPage.GetGdsSubmitButtonById("continue-button");

        // when
        var dashboardPage = await TestClient.SubmitButton(
            acceptAndSubmitButton,
            new Dictionary<string, string> { { "WithdrawReason", "very important reason" } });

        // then
        dashboardPage
            .UrlWithoutQueryEndsWith(PagesUrls.DashboardPage)
            .HasSuccessNotificationBanner("project has been withdrawn")
            .HasNotEmptyTitle();
    }

    private async Task<string> CreateNewApplicationDraft()
    {
        var applicationNamePage = await TestClient.NavigateTo(ApplicationPagesUrls.ApplicationName);

        var continueButton = applicationNamePage.GetGdsSubmitButtonById("continue-button");
        var taskListPage = await TestClient.SubmitButton(
            continueButton,
            ("LoanApplicationName", $"Application-{Guid.NewGuid()}"));

        var applicationToWithdrawId = taskListPage.Url.GetApplicationGuidFromUrl();
        return applicationToWithdrawId;
    }
}
