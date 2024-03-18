using System.Diagnostics.CodeAnalysis;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Projects.ViewModels;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.Project.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order09PlanningPermissionStatusIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order09PlanningPermissionStatusIntegrationTests(LoansIntegrationTestFixture fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldRedirectToLocation_WhenNoStatusWasSelected()
    {
        // given
        var planningStatusPage = await TestClient.NavigateTo(ProjectPagesUrls.PlanningPermissionStatus(_applicationLoanId, _projectId));
        var continueButton = planningStatusPage.GetGdsSubmitButtonById("continue-button");

        // when
        var locationPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { nameof(ProjectViewModel.PlanningPermissionStatus), string.Empty } });

        // then
        locationPage
            .UrlEndWith(ProjectPagesUrls.LocationSuffix)
            .HasTitle(ProjectPageTitles.Location);

        SetCurrentPage(planningStatusPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldRedirectToLocation_WhenAnyStatusWasSelected()
    {
        // given
        var planningStatusPage = await GetCurrentPage(() => TestClient.NavigateTo(ProjectPagesUrls.PlanningPermissionStatus(_applicationLoanId, _projectId)));
        var continueButton = planningStatusPage.GetGdsSubmitButtonById("continue-button");

        // when
        var locationPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { nameof(ProjectViewModel.PlanningPermissionStatus), ProjectFormOption.PlanningPermissionStatusOptions.NotSubmitted } });

        // then
        locationPage
            .UrlEndWith(ProjectPagesUrls.LocationSuffix)
            .HasTitle(ProjectPageTitles.Location);
    }
}
