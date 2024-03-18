using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.Common.Messages;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.FundingV2.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;
using CommonResponse = He.AspNetCore.Mvc.Gds.Components.Constants.CommonResponse;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.FundingSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order05PrivateSectorFundingIntegrationTests : IntegrationTest
{
    public Order05PrivateSectorFundingIntegrationTests(LoansIntegrationTestFixture fixture)
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
            continueButton, new Dictionary<string, string> { { "PrivateSectorFunding", CommonResponse.Yes }, { "PrivateSectorFundingResult", TextTestData.TextThatExceedsLongInputLimit }, { "PrivateSectorFundingReason", string.Empty } });

        // then
        privateSectorFundingPage
            .UrlEndWith(FundingPageUrls.PrivateSectorFundingSuffix)
            .HasTitle(FundingPageTitles.PrivateSectorFunding)
            .ContainsValidationMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.PrivateSectorFundingResult));
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
            continueButton, new Dictionary<string, string> { { "PrivateSectorFunding", CommonResponse.No }, { "PrivateSectorFundingResult", string.Empty }, { "PrivateSectorFundingReason", TextTestData.TextThatExceedsLongInputLimit } });

        // then
        privateSectorFundingPage
            .UrlEndWith(FundingPageUrls.PrivateSectorFundingSuffix)
            .HasTitle(FundingPageTitles.PrivateSectorFunding)
            .ContainsValidationMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.PrivateSectorFundingReason));
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
            continueButton, new Dictionary<string, string> { { "PrivateSectorFunding", CommonResponse.No }, { "PrivateSectorFundingResult", string.Empty }, { "PrivateSectorFundingReason", TextTestData.TextThatNotExceedsLongInputLimit } });

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
            continueButton, new Dictionary<string, string> { { "PrivateSectorFunding", CommonResponse.Yes }, { "PrivateSectorFundingResult", TextTestData.TextThatNotExceedsLongInputLimit }, { "PrivateSectorFundingReason", string.Empty } });

        // then
        repaymentSystemPage
            .UrlEndWith(FundingPageUrls.RepaymentSystemSuffix)
            .HasTitle(FundingPageTitles.RepaymentSystem);
    }
}
