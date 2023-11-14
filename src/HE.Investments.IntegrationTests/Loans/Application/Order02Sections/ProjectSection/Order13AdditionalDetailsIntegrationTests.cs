using System.Diagnostics.CodeAnalysis;
using HE.InvestmentLoans.Common.Tests.TestData;
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
public class Order13AdditionalDetailsIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order13AdditionalDetailsIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldShowValidationError_WhenNoPurchaseDateWasProvided()
    {
        // given
        var ownershipPage = await TestClient.NavigateTo(ProjectPagesUrls.AdditionalDetails(_applicationLoanId, _projectId));

        var continueButton = ownershipPage.GetGdsSubmitButtonById("continue-button");

        // when
        var grantFunding = await TestClient.SubmitButton(
                continueButton,
                (nameof(ProjectViewModel.PurchaseDay), string.Empty),
                (nameof(ProjectViewModel.PurchaseMonth), string.Empty),
                (nameof(ProjectViewModel.PurchaseYear), string.Empty));

        // then
        grantFunding
            .UrlEndWith(ProjectPagesUrls.AdditionalDetailsSuffix)
            .HasTitle(ProjectPageTitles.AdditionalDetails)
            .ContainsValidationMessage(ValidationErrorMessage.NoPurchaseDate);

        SetCurrentPage(ownershipPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldShowValidationError_WhenInvalidPurchaseDateWasProvided()
    {
        // given
        var ownershipPage = await TestClient.NavigateTo(ProjectPagesUrls.AdditionalDetails(_applicationLoanId, _projectId));

        var continueButton = ownershipPage.GetGdsSubmitButtonById("continue-button");

        var (year, month, day) = DateTimeTestData.IncorrectDateAsStrings;

        // when
        var grantFunding = await TestClient.SubmitButton(
                continueButton,
                (nameof(ProjectViewModel.PurchaseDay), day),
                (nameof(ProjectViewModel.PurchaseMonth), month),
                (nameof(ProjectViewModel.PurchaseYear), year));

        // then
        grantFunding
            .UrlEndWith(ProjectPagesUrls.AdditionalDetailsSuffix)
            .HasTitle(ProjectPageTitles.AdditionalDetails)
            .ContainsValidationMessage(ValidationErrorMessage.IncorrectPurchaseDate);

        SetCurrentPage(ownershipPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldShowValidationError_WhenCostWasNotProvided()
    {
        // given
        var ownershipPage = await TestClient.NavigateTo(ProjectPagesUrls.AdditionalDetails(_applicationLoanId, _projectId));

        var continueButton = ownershipPage.GetGdsSubmitButtonById("continue-button");

        var (year, month, day) = DateTimeTestData.CorrectDateAsStrings;

        // when
        var grantFunding = await TestClient.SubmitButton(
                continueButton,
                (nameof(ProjectViewModel.PurchaseDay), day),
                (nameof(ProjectViewModel.PurchaseMonth), month),
                (nameof(ProjectViewModel.PurchaseYear), year),
                (nameof(ProjectViewModel.Cost), string.Empty));

        // then
        grantFunding
            .UrlEndWith(ProjectPagesUrls.AdditionalDetailsSuffix)
            .HasTitle(ProjectPageTitles.AdditionalDetails)
            .ContainsValidationMessage(ValidationErrorMessage.IncorrectProjectCost);

        SetCurrentPage(ownershipPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldShowValidationError_WhenCurrentValueWasNotProvided()
    {
        // given
        var ownershipPage = await TestClient.NavigateTo(ProjectPagesUrls.AdditionalDetails(_applicationLoanId, _projectId));

        var continueButton = ownershipPage.GetGdsSubmitButtonById("continue-button");

        var (year, month, day) = DateTimeTestData.CorrectDateAsStrings;

        // when
        var grantFunding = await TestClient.SubmitButton(
                continueButton,
                (nameof(ProjectViewModel.PurchaseDay), day),
                (nameof(ProjectViewModel.PurchaseMonth), month),
                (nameof(ProjectViewModel.PurchaseYear), year),
                (nameof(ProjectViewModel.Cost), PoundsTestData.CorrectAmountAsString),
                (nameof(ProjectViewModel.Value), string.Empty));

        // then
        grantFunding
            .UrlEndWith(ProjectPagesUrls.AdditionalDetailsSuffix)
            .HasTitle(ProjectPageTitles.AdditionalDetails)
            .ContainsValidationMessage(ValidationErrorMessage.IncorrectProjectValue);

        SetCurrentPage(ownershipPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldShowValidationError_WhenNoSourceOfValuationWasSelected()
    {
        // given
        var ownershipPage = await TestClient.NavigateTo(ProjectPagesUrls.AdditionalDetails(_applicationLoanId, _projectId));

        var continueButton = ownershipPage.GetGdsSubmitButtonById("continue-button");

        var (year, month, day) = DateTimeTestData.CorrectDateAsStrings;

        // when
        var grantFunding = await TestClient.SubmitButton(
                continueButton,
                (nameof(ProjectViewModel.PurchaseDay), day),
                (nameof(ProjectViewModel.PurchaseMonth), month),
                (nameof(ProjectViewModel.PurchaseYear), year),
                (nameof(ProjectViewModel.Cost), PoundsTestData.CorrectAmountAsString),
                (nameof(ProjectViewModel.Value), PoundsTestData.CorrectAmountAsString),
                (nameof(ProjectViewModel.Source), string.Empty));

        // then
        grantFunding
            .UrlEndWith(ProjectPagesUrls.AdditionalDetailsSuffix)
            .HasTitle(ProjectPageTitles.AdditionalDetails)
            .ContainsValidationMessage(ValidationErrorMessage.EnterMoreDetails);

        SetCurrentPage(ownershipPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldShowAllValidationErrors_WhenNoData()
    {
        // given
        var ownershipPage = await TestClient.NavigateTo(ProjectPagesUrls.AdditionalDetails(_applicationLoanId, _projectId));

        var continueButton = ownershipPage.GetGdsSubmitButtonById("continue-button");

        // when
        var grantFunding = await TestClient.SubmitButton(
                continueButton,
                (nameof(ProjectViewModel.PurchaseDay), string.Empty),
                (nameof(ProjectViewModel.PurchaseMonth), string.Empty),
                (nameof(ProjectViewModel.PurchaseYear), string.Empty),
                (nameof(ProjectViewModel.Cost), string.Empty),
                (nameof(ProjectViewModel.Value), string.Empty),
                (nameof(ProjectViewModel.Source), string.Empty));

        // then
        grantFunding
            .UrlEndWith(ProjectPagesUrls.AdditionalDetailsSuffix)
            .HasTitle(ProjectPageTitles.AdditionalDetails)
            .ContainsValidationMessages(
                ValidationErrorMessage.NoPurchaseDate,
                ValidationErrorMessage.IncorrectProjectCost,
                ValidationErrorMessage.IncorrectProjectValue,
                ValidationErrorMessage.EnterMoreDetails);

        SetCurrentPage(ownershipPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ShouldRedirectToGrantFundingExists_WhenAllDataIsCorrect()
    {
        // given
        var ownershipPage = await TestClient.NavigateTo(ProjectPagesUrls.AdditionalDetails(_applicationLoanId, _projectId));

        var continueButton = ownershipPage.GetGdsSubmitButtonById("continue-button");

        var (year, month, day) = DateTimeTestData.CorrectDateAsStrings;

        // when
        var grantFunding = await TestClient.SubmitButton(
                continueButton,
                (nameof(ProjectViewModel.PurchaseDay), day),
                (nameof(ProjectViewModel.PurchaseMonth), month),
                (nameof(ProjectViewModel.PurchaseYear), year),
                (nameof(ProjectViewModel.Cost), PoundsTestData.CorrectAmountAsString),
                (nameof(ProjectViewModel.Value), PoundsTestData.CorrectAmountAsString),
                (nameof(ProjectViewModel.Source), SourceOfValuationTestData.AnySourceAsString));

        // then
        grantFunding
            .UrlEndWith(ProjectPagesUrls.GrantFundingExistsSuffix)
            .HasTitle(ProjectPageTitles.GrantFundingExists);

        SetCurrentPage(ownershipPage);
    }
}
