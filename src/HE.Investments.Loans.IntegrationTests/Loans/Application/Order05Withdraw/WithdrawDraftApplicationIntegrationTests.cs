using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW.Views.LoanApplicationV2.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order05Withdraw;

[Order(5)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class WithdrawDraftApplicationIntegrationTests : IntegrationTest
{
    public WithdrawDraftApplicationIntegrationTests(LoansIntegrationTestFixture fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldOpenWithdrawPage_WhenApplicationIsInDraftState()
    {
        // given
        var applicationToWithdrawId = await CreateNewApplicationDraft();

        var applicationDashboardPage =
            await TestClient.NavigateTo(ApplicationPagesUrls.ApplicationDashboard(UserOrganisationData.OrganisationId, applicationToWithdrawId));

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
            new Dictionary<string, string> { { "WithdrawReason", TextTestData.TextThatNotExceedsLongInputLimit } });

        // then
        dashboardPage
            .UrlWithoutQueryEndsWith(PagesUrls.DashboardPage(UserOrganisationData.OrganisationId))
            .HasSuccessNotificationBanner("project has been withdrawn")
            .HasNotEmptyTitle();
    }

    private async Task<string> CreateNewApplicationDraft()
    {
        var applicationNamePage = await TestClient.NavigateTo(ApplicationPagesUrls.ApplicationName(UserOrganisationData.OrganisationId));

        var continueButton = applicationNamePage.GetGdsSubmitButtonById("continue-button");
        var taskListPage = await TestClient.SubmitButton(
            continueButton,
            ("LoanApplicationName", $"Application-{Guid.NewGuid()}"));

        var applicationToWithdrawId = taskListPage.Url.GetApplicationGuidFromUrl();
        return applicationToWithdrawId;
    }
}
