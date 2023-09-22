using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.TestData;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.InvestmentLoans.WWW.Views.FundingV2.Consts;
using Xunit;
using Xunit.Extensions.Ordering;
using FormOptions = HE.InvestmentLoans.Common.Utils.Constants.FormOption;

namespace HE.InvestmentLoans.IntegrationTests.Loans.FundingSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order05PrivateSectorFundingIntegrationTests : IntegrationTest
{
    public Order05PrivateSectorFundingIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenYesIsSelectedAndApplyResultIsNotProvided()
    {
        // given
        var privateSectorFundingPage = await TestClient.NavigateTo(FundingPageUrls.PrivateSectorFunding(UserData.LoanApplicationIdInDraftState));
        var continueButton = privateSectorFundingPage.GetGdsSubmitButtonById("continue-button");

        // when
        privateSectorFundingPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "PrivateSectorFunding", CommonResponse.Yes }, { "PrivateSectorFundingResult", string.Empty }, { "PrivateSectorFundingReason", string.Empty } });

        // then
        privateSectorFundingPage
            .UrlEndWith(FundingPageUrls.PrivateSectorFundingSuffix)
            .HasTitle(FundingPageTitles.PrivateSectorFunding)
            .ContainsValidationMessage(ValidationErrorMessage.EnterMoreDetails);

        SetSharedData(SharedKeys.CurrentPageKey, privateSectorFundingPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldDisplayValidationError_WhenYesIsSelectedAndApplyResultIsLongerThan1500Characters()
    {
        // given
        var privateSectorFundingPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);

        var continueButton = privateSectorFundingPage.GetGdsSubmitButtonById("continue-button");

        // when
        privateSectorFundingPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "PrivateSectorFunding", CommonResponse.Yes }, { "PrivateSectorFundingResult", TextTestData.TextWithLenght1501 }, { "PrivateSectorFundingReason", string.Empty } });

        // then
        privateSectorFundingPage
            .UrlEndWith(FundingPageUrls.PrivateSectorFundingSuffix)
            .HasTitle(FundingPageTitles.PrivateSectorFunding)
            .ContainsValidationMessage(ValidationErrorMessage.LongInputLengthExceeded(FormOptions.FieldNameForInputLengthValidation.PrivateSectorFundingResult));
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldDisplayValidationError_WhenNoIsSelectedAndNotApplyingReasonIsNotProvided()
    {
        // given
        var privateSectorFundingPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = privateSectorFundingPage.GetGdsSubmitButtonById("continue-button");

        // when
        privateSectorFundingPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "PrivateSectorFunding", CommonResponse.No }, { "PrivateSectorFundingResult", string.Empty }, { "PrivateSectorFundingReason", string.Empty } });

        // then
        privateSectorFundingPage
            .UrlEndWith(FundingPageUrls.PrivateSectorFundingSuffix)
            .HasTitle(FundingPageTitles.PrivateSectorFunding)
            .ContainsValidationMessage(ValidationErrorMessage.EnterMoreDetails);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldDisplayValidationError_WhenNoIsSelectedAndNotApplyingReasonIsLongerThan1500Characters()
    {
        // given
        var privateSectorFundingPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);

        var continueButton = privateSectorFundingPage.GetGdsSubmitButtonById("continue-button");

        // when
        privateSectorFundingPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "PrivateSectorFunding", CommonResponse.No }, { "PrivateSectorFundingResult", string.Empty }, { "PrivateSectorFundingReason", TextTestData.TextWithLenght1501 } });

        // then
        privateSectorFundingPage
            .UrlEndWith(FundingPageUrls.PrivateSectorFundingSuffix)
            .HasTitle(FundingPageTitles.PrivateSectorFunding)
            .ContainsValidationMessage(ValidationErrorMessage.LongInputLengthExceeded(FormOptions.FieldNameForInputLengthValidation.PrivateSectorFundingReason));
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldMoveToRepaymentSystem_WhenNoIsSelectedAndNotApplyingReasonIsProvided()
    {
        // given
        var privateSectorFundingPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = privateSectorFundingPage.GetGdsSubmitButtonById("continue-button");

        // when
        var repaymentSystemPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "PrivateSectorFunding", CommonResponse.No }, { "PrivateSectorFundingResult", string.Empty }, { "PrivateSectorFundingReason", TextTestData.TextWithLenght1000 } });

        // then
        repaymentSystemPage
            .UrlEndWith(FundingPageUrls.RepaymentSystemSuffix)
            .HasTitle(FundingPageTitles.RepaymentSystem);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldMoveToRepaymentSystem_WhenYesIsSelectedAndApplyResultIsProvided()
    {
        // given
        var privateSectorFundingPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = privateSectorFundingPage.GetGdsSubmitButtonById("continue-button");

        // when
        var repaymentSystemPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "PrivateSectorFunding", CommonResponse.Yes }, { "PrivateSectorFundingResult", TextTestData.TextWithLenght1000 }, { "PrivateSectorFundingReason", string.Empty } });

        // then
        repaymentSystemPage
            .UrlEndWith(FundingPageUrls.RepaymentSystemSuffix)
            .HasTitle(FundingPageTitles.RepaymentSystem);
    }
}
