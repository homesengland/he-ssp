using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.InvestmentLoans.WWW.Views.Project.Consts;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order09PlanningPermissionStatusIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order09PlanningPermissionStatusIntegrationTests(IntegrationTestFixture<Program> fixture)
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
