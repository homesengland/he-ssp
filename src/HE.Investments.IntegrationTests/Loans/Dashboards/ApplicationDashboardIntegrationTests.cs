using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.Dashboards;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class ApplicationDashboardIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;

    public ApplicationDashboardIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = GetSharedData<string>(SharedKeys.ApplicationLoanIdInDraftStatusKey);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldOpenApplicationDashboard()
    {
        // given
        var dashboardPage = await TestClient.NavigateTo(PagesUrls.DashboardPage);

        // when
        var applicationLink = dashboardPage.GetAnchorElementById($"open-application-link-{_applicationLoanId}");
        var applicationDashboardPage = await TestClient.NavigateTo(applicationLink);

        // then
        applicationDashboardPage
            .UrlEndWith(ApplicationPagesUrls.ApplicationDashboard(_applicationLoanId))
            .HasNotEmptyTitle();

        SetSharedData(SharedKeys.CurrentPageKey, applicationDashboardPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldRedirectToTaskList()
    {
        // given
        var applicationDashboardPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);

        // when
        var editApplicationLink = applicationDashboardPage.GetAnchorElementById("edit-link-application-details");
        var taskListPage = await TestClient.NavigateTo(editApplicationLink);

        // then
        taskListPage
            .UrlEndWith(ApplicationPagesUrls.TaskListSuffix)
            .HasTitle("Development loan application");
    }
}
