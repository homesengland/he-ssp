using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.CompanyStructureV2.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.CompanyStructureSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01StartCompanyStructureIntegrationTests : IntegrationTest
{
    public Order01StartCompanyStructureIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldOpenCompanyStructureStartingPage_WhenCompanyStructureLinkIsClickedOnTaskListPage()
    {
        // given
        var taskList = await TestClient.NavigateTo(ApplicationPagesUrls.TaskList(UserData.LoanApplicationIdInDraftState));

        // when
        var linkToCompanyStructureSection = taskList.GetAnchorElementById("company-structure-section-link");
        var startCompanyStructurePage = await TestClient.NavigateTo(linkToCompanyStructureSection);

        // then
        startCompanyStructurePage
            .UrlEndWith(CompanyStructurePagesUrls.StartCompanyStructureSuffix)
            .HasTitle(CompanyStructurePageTitles.Start)
            .HasGdsSubmitButton("start-now-button", out _);

        SetSharedData(SharedKeys.CurrentPageKey, startCompanyStructurePage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldMoveToNextPageCompanyPurpose_WhenStartButtonIsClicked()
    {
        // given
        var startCompanyStructurePage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var startNow = startCompanyStructurePage.GetGdsSubmitButtonById("start-now-button");

        // when
        var companyPurposePage = await TestClient.SubmitButton(startNow);

        // then
        companyPurposePage
            .UrlEndWith(CompanyStructurePagesUrls.CompanyPurposeSuffix)
            .HasTitle("Was your organisation established specifically for this development?");
    }
}
