using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.Security.Consts;
using Xunit;
using Xunit.Extensions.Ordering;
using CommonResponse = He.AspNetCore.Mvc.Gds.Components.Constants.CommonResponse;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.SecuritySection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order02ChargesDebtIntegrationTests : IntegrationTest
{
    public Order02ChargesDebtIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenYesIsSelectedAndAdditionalInformationIsNotProvided()
    {
        // given
        var chargesDebtPage = await TestClient.NavigateTo(SecurityPageUrls.ChargesDebt(UserData.LoanApplicationIdInDraftState));
        var continueButton = chargesDebtPage.GetGdsSubmitButtonById("continue-button");

        // when
        chargesDebtPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "ChargesDebtCompany", CommonResponse.Yes }, { "ChargesDebtCompanyInfo", string.Empty } });

        // then
        chargesDebtPage
            .UrlEndWith(SecurityPageUrls.ChargesDebtSuffix)
            .HasTitle(SecurityPageTitles.ChargesDebt)
            .ContainsOnlyOneValidationMessage(ValidationErrorMessage.EnterMoreDetails);

        SetSharedData(SharedKeys.CurrentPageKey, chargesDebtPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldDisplayValidationError_WhenNoIsSelectedAndAdditionalInformationIsLongerThan1500Characters()
    {
        // given
        var chargesDebtPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);

        var continueButton = chargesDebtPage.GetGdsSubmitButtonById("continue-button");

        // when
        var directorLoansPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "ChargesDebtCompany", CommonResponse.Yes }, { "ChargesDebtCompanyInfo", TextTestData.TextThatExceedsLongInputLimit } });

        // then
        directorLoansPage
            .UrlEndWith(SecurityPageUrls.ChargesDebtSuffix)
            .HasTitle(SecurityPageTitles.ChargesDebt)
            .ContainsOnlyOneValidationMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.Debenture));
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldMoveToDirectorLoans_WhenNoIsSelected()
    {
        // given
        var chargesDebtPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = chargesDebtPage.GetGdsSubmitButtonById("continue-button");

        // when
        var directorLoansPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "ChargesDebtCompany", CommonResponse.No }, { "ChargesDebtCompanyInfo", string.Empty } });

        // then
        directorLoansPage
            .UrlEndWith(SecurityPageUrls.DirectorLoansSuffix)
            .HasTitle(SecurityPageTitles.DirLoans);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldMoveToDirectorLoans_WhenYesIsSelectedAndAdditionalDataProvided()
    {
        // given
        var chargesDebtPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = chargesDebtPage.GetGdsSubmitButtonById("continue-button");

        // when
        var directorLoansPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "ChargesDebtCompany", CommonResponse.Yes }, { "ChargesDebtCompanyInfo", TextTestData.TextThatNotExceedsLongInputLimit } });

        // then
        directorLoansPage
            .UrlEndWith(SecurityPageUrls.DirectorLoansSuffix)
            .HasTitle(SecurityPageTitles.DirLoans);
    }
}
