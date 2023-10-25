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
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.Application.Order04Withdraw;

[Order(3)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class WithdrawDraftApplicationIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;

    public WithdrawDraftApplicationIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.SubmittedLoanApplicationId;
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

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldOpenWithdrawPage_WhenApplicationIsInDraftState()
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
            .UrlEndWith(PagesUrls.DashboardPage)
            .HasSuccessNotificationBanner("project has been withdrawn")
            .HasNotEmptyTitle();
    }
}
