using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order01StartApplication;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order02ApplicationDashboardIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;

    public Order02ApplicationDashboardIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
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
            .HasTitle(UserData.LoanApplicationName);
    }
}
