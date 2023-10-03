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
public class Order11OwnershipIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order11OwnershipIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldRedirectToGrantFunding_WhenNoWasSelected()
    {
        // given
        var ownershipPage = await GetCurrentPage(ProjectPagesUrls.Ownership(_applicationLoanId, _projectId));

        var continueButton = ownershipPage.GetGdsSubmitButtonById("continue-button");

        // when
        var grantFunding = await TestClient.SubmitButton(continueButton, (nameof(ProjectViewModel.Ownership), CommonResponse.No));

        // then
        grantFunding
            .UrlEndWith(ProjectPagesUrls.GrantFundingExistsSuffix)
            .HasTitle(ProjectPageTitles.GrantFundingExists);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldRedirectToAdditionalDetails_WhenNoWasSelected()
    {
        // given
        var ownershipPage = await GetCurrentPage(ProjectPagesUrls.Ownership(_applicationLoanId, _projectId));

        var continueButton = ownershipPage.GetGdsSubmitButtonById("continue-button");

        // when
        var grantFunding = await TestClient.SubmitButton(continueButton, (nameof(ProjectViewModel.Ownership), CommonResponse.Yes));

        // then
        grantFunding
            .UrlEndWith(ProjectPagesUrls.AdditionalDetailsSuffix)
            .HasTitle(ProjectPageTitles.AdditionalDetails);
    }
}
