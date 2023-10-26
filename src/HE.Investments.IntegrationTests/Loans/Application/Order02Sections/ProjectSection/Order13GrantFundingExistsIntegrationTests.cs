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

namespace HE.InvestmentLoans.IntegrationTests.Loans.Application.Order02Sections.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order13GrantFundingExistsIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order13GrantFundingExistsIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldRedirectToChargesDebt_WhenNoWasSelected()
    {
        // given
        var ownershipPage = await TestClient.NavigateTo(ProjectPagesUrls.GrantFundingExists(_applicationLoanId, _projectId));

        var continueButton = ownershipPage.GetGdsSubmitButtonById("continue-button");

        // when
        var grantFunding = await TestClient.SubmitButton(continueButton, (nameof(ProjectViewModel.GrantFundingStatus), CommonResponse.No));

        // then
        grantFunding
            .UrlEndWith(ProjectPagesUrls.ChargesDebtSuffix)
            .HasTitle(ProjectPageTitles.ChargesDebt);

        SetCurrentPage(ownershipPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldRedirectToChargesDebt_WhenDoNotKnowWasSelected()
    {
        // given
        var ownershipPage = await GetCurrentPage(ProjectPagesUrls.GrantFundingExists(_applicationLoanId, _projectId));

        var continueButton = ownershipPage.GetGdsSubmitButtonById("continue-button");

        // when
        var grantFunding = await TestClient.SubmitButton(continueButton, (nameof(ProjectViewModel.GrantFundingStatus), CommonResponse.DoNotKnow));

        // then
        grantFunding
            .UrlEndWith(ProjectPagesUrls.ChargesDebtSuffix)
            .HasTitle(ProjectPageTitles.ChargesDebt);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldRedirectToGrantFunding_WhenYesWasSelected()
    {
        // given
        var ownershipPage = await GetCurrentPage(ProjectPagesUrls.GrantFundingExists(_applicationLoanId, _projectId));

        var continueButton = ownershipPage.GetGdsSubmitButtonById("continue-button");

        // when
        var grantFunding = await TestClient.SubmitButton(continueButton, (nameof(ProjectViewModel.GrantFundingStatus), CommonResponse.Yes));

        // then
        grantFunding
            .UrlEndWith(ProjectPagesUrls.GrantFundingSuffix)
            .HasTitle(ProjectPageTitles.GrantFunding);
    }
}
