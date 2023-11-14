using System.Diagnostics.CodeAnalysis;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.InvestmentLoans.WWW.Views.Project.Consts;
using HE.Investments.Common.Messages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.Application.Order02Sections.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order16ChargesDebtIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order16ChargesDebtIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldShowValidationMessage_WhenYesIsSelectedButNoAdditionalInformationIsProvided()
    {
        // given
        var chargesDebtPage = await TestClient.NavigateTo(ProjectPagesUrls.ChargesDebt(_applicationLoanId, _projectId));
        var continueButton = chargesDebtPage.GetGdsSubmitButtonById("continue-button");

        // when
        chargesDebtPage = await TestClient.SubmitButton(continueButton, (nameof(ProjectViewModel.ChargesDebt), CommonResponse.Yes), (nameof(ProjectViewModel.ChargesDebtInfo), string.Empty));

        // then
        chargesDebtPage
            .UrlEndWith(ProjectPagesUrls.ChargesDebtSuffix)
            .HasTitle(ProjectPageTitles.ChargesDebt)
            .HasOneValidationMessages(ValidationErrorMessage.EnterExistingLegal);

        SetCurrentPage(chargesDebtPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldShowValidationMessage_WhenYesIsSelectedAndAdditionalInformationIsTooLong()
    {
        // given
        var chargesDebtPage = await TestClient.NavigateTo(ProjectPagesUrls.ChargesDebt(_applicationLoanId, _projectId));
        var continueButton = chargesDebtPage.GetGdsSubmitButtonById("continue-button");

        // when
        chargesDebtPage = await TestClient.SubmitButton(continueButton, (nameof(ProjectViewModel.ChargesDebt), CommonResponse.Yes), (nameof(ProjectViewModel.ChargesDebtInfo), TextTestData.TextThatExceedsLongInputLimit));

        // then
        chargesDebtPage
            .UrlEndWith(ProjectPagesUrls.ChargesDebtSuffix)
            .HasTitle(ProjectPageTitles.ChargesDebt)
            .HasOneValidationMessages(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.ChargesDebtInfo));
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldRedirectToAffordableHomes_WhenYesIsSelectedAndCorrectAdditionalInformationIsProvided()
    {
        // given
        var chargesDebtPage = await TestClient.NavigateTo(ProjectPagesUrls.ChargesDebt(_applicationLoanId, _projectId));
        var continueButton = chargesDebtPage.GetGdsSubmitButtonById("continue-button");

        // when
        var affordableHomesSuffix = await TestClient.SubmitButton(continueButton, (nameof(ProjectViewModel.ChargesDebt), CommonResponse.Yes), (nameof(ProjectViewModel.ChargesDebtInfo), TextTestData.TextThatNotExceedsLongInputLimit));

        // then
        affordableHomesSuffix
            .UrlEndWith(ProjectPagesUrls.AffordableHomesSuffix)
            .HasTitle(ProjectPageTitles.AffordableHomes);
    }
}
