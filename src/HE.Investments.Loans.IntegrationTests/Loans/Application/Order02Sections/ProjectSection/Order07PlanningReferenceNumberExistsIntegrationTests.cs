using System.Diagnostics.CodeAnalysis;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
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
