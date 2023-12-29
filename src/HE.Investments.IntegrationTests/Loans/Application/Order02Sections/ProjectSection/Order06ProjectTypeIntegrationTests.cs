using System.Diagnostics.CodeAnalysis;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.IntegrationTestsFramework.Extensions;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.Project.Consts;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order06ProjectTypeIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order06ProjectTypeIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Order(1)]
    [Fact(Skip = LoansConfig.SkipTest)]
    public async Task Order01_ShouldRedirectToPlanningReferenceNumberExists_WhenAnyOptionIsSelected()
    {
        // given
        var projectTypePage = await TestClient.NavigateTo(ProjectPagesUrls.ProjectType(_applicationLoanId, _projectId));
        var continueButton = projectTypePage.GetGdsSubmitButtonById("continue-button");

        // when
        var refNumberExistsPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "ProjectType", "greenfield" } });

        // then
        refNumberExistsPage
            .UrlEndWith(ProjectPagesUrls.PlanningReferenceNumberExistsSuffix)
            .HasTitle(ProjectPageTitles.PlanningReferenceNumberExists);
    }
}
