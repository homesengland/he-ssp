using System.Diagnostics.CodeAnalysis;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.InvestmentLoans.WWW.Views.Project.Consts;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.Application.Order02Sections.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order17AffordableHomesIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order17AffordableHomesIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldRedirectToCheckAnswers_WhenAnyOptionIsSelected()
    {
        // given
        var affordableHomesPage = await TestClient.NavigateTo(ProjectPagesUrls.AffordableHomes(_applicationLoanId, _projectId));
        var continueButton = affordableHomesPage.GetGdsSubmitButtonById("continue-button");

        // when
        var checkAnswersPage = await TestClient.SubmitButton(continueButton, (nameof(ProjectViewModel.AffordableHomes), CommonResponse.Yes));

        // then
        checkAnswersPage
            .UrlEndWith(ProjectPagesUrls.CheckAnswersSuffix)
            .HasTitle(ProjectPageTitles.CheckAnswers);
    }
}
