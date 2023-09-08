using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.CompanyStructureSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01StartCompanyStructureIntegrationTests : IntegrationTest
{
    private const string CurrentPageKey = nameof(CurrentPageKey);

    public Order01StartCompanyStructureIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldOpenCompanyStructureStartingPage_WhenCompanyStructureLinkIsClickedOnTaskListPage()
    {
        // given
        var taskList = await TestClient.NavigateTo(ApplicationPagesUrls.TaskList("2b018098-7b4d-ee11-be6f-002248c653e1"));

        // when
        var linkToCompanyStructureSection = taskList.GetAnchorElementById("company-structure-section-link");
        var startCompanyStructurePage = await TestClient.NavigateTo(linkToCompanyStructureSection);

        // then
        startCompanyStructurePage.Url.Should().EndWith(CompanyStructurePagesUrls.StartCompanyStructureSuffix);
        startCompanyStructurePage.GetPageTitle().Should().Be("Company structure and experience");
        startCompanyStructurePage.GetGdsSubmitButtonById("start-now-button");

        SetSharedData(CurrentPageKey, startCompanyStructurePage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldMoveToNextPageCompanyPurpose_WhenStartButtonIsClicked()
    {
        // given
        var startCompanyStructurePage = GetSharedData<IHtmlDocument>(CurrentPageKey);
        var startNow = startCompanyStructurePage.GetGdsSubmitButtonById("start-now-button");

        // when
        var companyPurposePage = await TestClient.SubmitButton(startNow);

        // then
        companyPurposePage.Url.Should().EndWith(CompanyStructurePagesUrls.CompanyPurposeSuffix);
        companyPurposePage.GetPageTitle().Should().Be("Was your organisation established specifically for this development?");
        SetSharedData(CurrentPageKey, companyPurposePage);
    }
}
