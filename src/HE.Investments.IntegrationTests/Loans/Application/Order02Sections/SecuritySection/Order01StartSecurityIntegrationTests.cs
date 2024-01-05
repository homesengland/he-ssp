using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.Security.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.SecuritySection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01StartSecurityIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;

    public Order01StartSecurityIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldOpenCompanyStructureStartingPage_WhenCompanyStructureLinkIsClickedOnTaskListPage()
    {
        // given
        var taskList = await TestClient.NavigateTo(ApplicationPagesUrls.TaskList(_applicationLoanId));

        // when
        var linkToSecuritySection = taskList.GetAnchorElementById("security-section-link");
        var startSecurityPage = await TestClient.NavigateTo(linkToSecuritySection);

        // then
        startSecurityPage
            .UrlEndWith(SecurityPageUrls.StartSuffix)
            .HasTitle(SecurityPageTitles.Index)
            .HasGdsSubmitButton("start-now-button", out _);

        SetSharedData(SharedKeys.CurrentPageKey, startSecurityPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldMoveToNextPageCompanyPurpose_WhenStartButtonIsClicked()
    {
        // given
        var startSecurityPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var startNow = startSecurityPage.GetGdsSubmitButtonById("start-now-button");

        // when
        var companyPurposePage = await TestClient.SubmitButton(startNow);

        // then
        companyPurposePage
            .UrlEndWith(SecurityPageUrls.ChargesDebtSuffix)
            .HasTitle(SecurityPageTitles.ChargesDebt);
    }
}
