using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.Application;

[Order(3)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class SubmitApplicationIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;

    public SubmitApplicationIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
    }

    [Fact(Skip = "LoansConfig.SkipTest")]
    [Order(1)]
    public async Task Order01_ShouldOpenCheckAllAnswersPage_WhenAllApplicationSectionAreFilled()
    {
        // given
        var taskList = await TestClient.NavigateTo(ApplicationPagesUrls.TaskList(_applicationLoanId));
        var submitLoanApplicationButton = taskList.GetGdsSubmitButtonById("submit-application");

        // when
        var checkApplicationPage = await TestClient.SubmitButton(submitLoanApplicationButton);

        // then
        checkApplicationPage
            .UrlEndWith(ApplicationPagesUrls.CheckApplicationSuffix)
            .HasTitle("Check your answers before submitting your application")
            .HasGdsSubmitButton("accept-and-submit", out _);

        SetSharedData(SharedKeys.CurrentPageKey, checkApplicationPage);
    }

    [Fact(Skip = "LoansConfig.SkipTest")]
    [Order(2)]
    public async Task Order02_ShouldMoveToApplicationSubmitted_WhenAcceptAndSubmitButtonIsClicked()
    {
        // given
        var checkApplicationPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var acceptAndSubmitButton = checkApplicationPage.GetGdsSubmitButtonById("accept-and-submit");

        // when
        var applicationSubmittedPage = await TestClient.SubmitButton(acceptAndSubmitButton);

        // then
        applicationSubmittedPage
            .UrlEndWith(ApplicationPagesUrls.ApplicationSubmittedSuffix)
            .HasGdsSubmitButton("application-submitted-to-dashboard", out _);
    }
}
