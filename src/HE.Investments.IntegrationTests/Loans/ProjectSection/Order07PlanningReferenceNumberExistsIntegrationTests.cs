using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.InvestmentLoans.WWW.Views.Project.Consts;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order07PlanningReferenceNumberExistsIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order07PlanningReferenceNumberExistsIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldRedirectToLocation_WhenNoIsSelected()
    {
        // given
        var planningReferenceNumberExistsPage = await TestClient.NavigateTo(ProjectPagesUrls.PlanningReferenceNumberExists(_applicationLoanId, _projectId));
        var continueButton = planningReferenceNumberExistsPage.GetGdsSubmitButtonById("continue-button");

        // when
        var planningReferenceNumber = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "PlanningReferenceNumberExists", CommonResponse.No } });

        // then
        planningReferenceNumber
            .UrlEndWith(ProjectPagesUrls.LocationSuffix)
            .HasTitle(ProjectPageTitles.Location);

        SetSharedData(SharedKeys.CurrentPageKey, planningReferenceNumberExistsPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldRedirectToPlanningReferenceNumber_WhenYesIsSelected()
    {
        // given
        var planningReferenceNumberExistsPage = await GetCurrentPage(() => TestClient.NavigateTo(ProjectPagesUrls.PlanningReferenceNumberExists(_applicationLoanId, _projectId)));
        var continueButton = planningReferenceNumberExistsPage.GetGdsSubmitButtonById("continue-button");

        // when
        var locationPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "PlanningReferenceNumberExists", CommonResponse.Yes } });

        // then
        locationPage
            .UrlEndWith(ProjectPagesUrls.PlanningReferenceNumberSuffix)
            .HasLabelTitle(ProjectPageTitles.PlanningReferenceNumber);
    }
}
