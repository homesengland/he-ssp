using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.FundingV2.Consts;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.FundingSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01StartFundingIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;

    public Order01StartFundingIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldOpenFundingStartingPage_WhenFundingLinkIsClickedOnTaskListPage()
    {
        // given
        var taskList = await TestClient.NavigateTo(ApplicationPagesUrls.TaskList(_applicationLoanId));

        // when
        var linkToFundingSection = taskList.GetAnchorElementById("funding-section-link");
        var startFundingPage = await TestClient.NavigateTo(linkToFundingSection);

        // then
        startFundingPage
            .UrlEndWith(FundingPageUrls.StartFundingSuffix)
            .HasTitle(FundingPageTitles.StartFunding)
            .HasGdsSubmitButton("start-now-button", out _);

        SetSharedData(SharedKeys.CurrentPageKey, startFundingPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldMoveToNextPageGrossDevelopmentValue_WhenStartButtonIsClicked()
    {
        // given
        var startFundingPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var startNow = startFundingPage.GetGdsSubmitButtonById("start-now-button");

        // when
        var grossDevelopmentValuePage = await TestClient.SubmitButton(startNow);

        // then
        grossDevelopmentValuePage
            .UrlEndWith(FundingPageUrls.GrossDevelopmentValueSuffix)
            .HasLabelTitle(FundingPageTitles.GrossDevelopmentValue);
    }
}
