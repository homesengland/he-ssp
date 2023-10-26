using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Common.Utils.Constants;
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
public class Order14GrantFundingIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order14GrantFundingIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_RedirectToChargesDebt_WhenNoDataIsProvided()
    {
        // given
        var grantFundingPage = await TestClient.NavigateTo(ProjectPagesUrls.GrantFunding(_applicationLoanId, _projectId));

        var continueButton = grantFundingPage.GetGdsSubmitButtonById("continue-button");

        // when
        var chargesDebtPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(ProjectViewModel.GrantFundingProviderName), string.Empty),
            (nameof(ProjectViewModel.GrantFundingAmount), string.Empty),
            (nameof(ProjectViewModel.GrantFundingName), string.Empty),
            (nameof(ProjectViewModel.GrantFundingPurpose), string.Empty));

        // then
        chargesDebtPage
            .UrlEndWith(ProjectPagesUrls.ChargesDebtSuffix)
            .HasTitle(ProjectPageTitles.ChargesDebt);

        SetCurrentPage(grantFundingPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShowValidationError_WhenProviderNameIsLongerThanShortInputLimit()
    {
        // given
        var grantFundingPage = await GetCurrentPage(ProjectPagesUrls.GrantFunding(_applicationLoanId, _projectId));

        var continueButton = grantFundingPage.GetGdsSubmitButtonById("continue-button");

        // when
        grantFundingPage = await TestClient.SubmitButton(continueButton, (nameof(ProjectViewModel.GrantFundingProviderName), TextTestData.TextThatExceedsShortInputLimit));

        // then
        grantFundingPage
            .UrlEndWith(ProjectPagesUrls.GrantFundingSuffix)
            .HasTitle(ProjectPageTitles.GrantFunding)
            .HasOneValidationMessages(ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.GrantFundingProviderName));
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShowValidationError_WhenIncorrectAmountIsProvided()
    {
        // given
        var grantFundingPage = await GetCurrentPage(ProjectPagesUrls.GrantFunding(_applicationLoanId, _projectId));

        var continueButton = grantFundingPage.GetGdsSubmitButtonById("continue-button");

        // when
        grantFundingPage = await TestClient.SubmitButton(continueButton, (nameof(ProjectViewModel.GrantFundingAmount), PoundsTestData.IncorrectAmountAsString), (nameof(ProjectViewModel.GrantFundingProviderName), string.Empty));

        // then
        grantFundingPage
            .UrlEndWith(ProjectPagesUrls.GrantFundingSuffix)
            .HasTitle(ProjectPageTitles.GrantFunding)
            .HasOneValidationMessages(ValidationErrorMessage.IncorrectGrantFundingAmount);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShowValidationError_WhenNameOfGrantExceedsShortInputLimit()
    {
        // given
        var grantFundingPage = await GetCurrentPage(ProjectPagesUrls.GrantFunding(_applicationLoanId, _projectId));

        var continueButton = grantFundingPage.GetGdsSubmitButtonById("continue-button");

        // when
        grantFundingPage = await TestClient.SubmitButton(continueButton, (nameof(ProjectViewModel.GrantFundingName), TextTestData.TextThatExceedsShortInputLimit), (nameof(ProjectViewModel.GrantFundingAmount), string.Empty), (nameof(ProjectViewModel.GrantFundingProviderName), string.Empty));

        // then
        grantFundingPage
            .UrlEndWith(ProjectPagesUrls.GrantFundingSuffix)
            .HasTitle(ProjectPageTitles.GrantFunding)
            .HasOneValidationMessages(ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.GrantFundingName));
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShowValidationError_WhenGrantPurposeExceedsLongInputLimit()
    {
        // given
        var grantFundingPage = await GetCurrentPage(ProjectPagesUrls.GrantFunding(_applicationLoanId, _projectId));

        var continueButton = grantFundingPage.GetGdsSubmitButtonById("continue-button");

        // when
        grantFundingPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(ProjectViewModel.GrantFundingPurpose), TextTestData.TextThatExceedsLongInputLimit),
            (nameof(ProjectViewModel.GrantFundingAmount), string.Empty),
            (nameof(ProjectViewModel.GrantFundingName), string.Empty),
            (nameof(ProjectViewModel.GrantFundingProviderName), string.Empty));

        // then
        grantFundingPage
            .UrlEndWith(ProjectPagesUrls.GrantFundingSuffix)
            .HasTitle(ProjectPageTitles.GrantFunding)
            .HasOneValidationMessages(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.GrantFundingPurpose));
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShowAllValidationErrors_WhenAllDataIsIncorrect()
    {
        // given
        var grantFundingPage = await GetCurrentPage(ProjectPagesUrls.GrantFunding(_applicationLoanId, _projectId));

        var continueButton = grantFundingPage.GetGdsSubmitButtonById("continue-button");

        // when
        grantFundingPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(ProjectViewModel.GrantFundingProviderName), TextTestData.TextThatExceedsShortInputLimit),
            (nameof(ProjectViewModel.GrantFundingAmount), PoundsTestData.IncorrectAmountAsString),
            (nameof(ProjectViewModel.GrantFundingName), TextTestData.TextThatExceedsShortInputLimit),
            (nameof(ProjectViewModel.GrantFundingPurpose), TextTestData.TextThatExceedsLongInputLimit));

        // then
        grantFundingPage
            .UrlEndWith(ProjectPagesUrls.GrantFundingSuffix)
            .HasTitle(ProjectPageTitles.GrantFunding)
            .HasValidationMessages(ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.GrantFundingProviderName))
            .HasValidationMessages(ValidationErrorMessage.IncorrectGrantFundingAmount)
            .HasValidationMessages(ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.GrantFundingName))
            .HasValidationMessages(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.GrantFundingPurpose));
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_RedirectToChargesDebt_WhenAllDataIsCorrect()
    {
        // given
        var grantFundingPage = await GetCurrentPage(ProjectPagesUrls.GrantFunding(_applicationLoanId, _projectId));

        var continueButton = grantFundingPage.GetGdsSubmitButtonById("continue-button");

        // when
        var chargesDebtPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(ProjectViewModel.GrantFundingProviderName), TextTestData.TextThatNotExceedsShortInputLimit),
            (nameof(ProjectViewModel.GrantFundingAmount), PoundsTestData.CorrectAmountAsString),
            (nameof(ProjectViewModel.GrantFundingName), TextTestData.TextThatNotExceedsShortInputLimit),
            (nameof(ProjectViewModel.GrantFundingPurpose), TextTestData.TextThatNotExceedsLongInputLimit));

        // then
        chargesDebtPage
            .UrlEndWith(ProjectPagesUrls.ChargesDebtSuffix)
            .HasTitle(ProjectPageTitles.ChargesDebt);
    }
}
