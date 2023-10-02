using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.InvestmentLoans.WWW.Views.FundingV2.Consts;
using Xunit;
using Xunit.Extensions.Ordering;
using FormOptions = HE.InvestmentLoans.Common.Utils.Constants.FormOption;

namespace HE.InvestmentLoans.IntegrationTests.Loans.FundingSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order06RepaymentSystemIntegrationTests : IntegrationTest
{
    public Order06RepaymentSystemIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenRefinanceIsSelectedAndAdditionalInformationIsNotProvided()
    {
        // given
        var repaymentSystemPage = await TestClient.NavigateTo(FundingPageUrls.RepaymentSystem(UserData.LoanApplicationIdInDraftState));
        var continueButton = repaymentSystemPage.GetGdsSubmitButtonById("continue-button");

        // when
        repaymentSystemPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "Refinance", FundingFormOption.Refinance }, { "RefinanceInfo", string.Empty } });

        // then
        repaymentSystemPage
            .UrlEndWith(FundingPageUrls.RepaymentSystemSuffix)
            .HasTitle(FundingPageTitles.RepaymentSystem)
            .ContainsValidationMessage(ValidationErrorMessage.EnterMoreDetails);

        SetSharedData(SharedKeys.CurrentPageKey, repaymentSystemPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldDisplayValidationError_WhenRefinanceIsSelectedAndAdditionalInformationIsLongerThan1500Characters()
    {
        // given
        var repaymentSystemPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);

        var continueButton = repaymentSystemPage.GetGdsSubmitButtonById("continue-button");

        // when
        repaymentSystemPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "Refinance", FundingFormOption.Refinance }, { "RefinanceInfo", TextTestData.TextWithLenght1501 } });

        // then
        repaymentSystemPage
            .UrlEndWith(FundingPageUrls.RepaymentSystemSuffix)
            .HasTitle(FundingPageTitles.RepaymentSystem)
            .ContainsValidationMessage(ValidationErrorMessage.LongInputLengthExceeded(FormOptions.FieldNameForInputLengthValidation.RefinanceInfo));
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldMoveToAdditionalProjects_WhenRepayIsSelected()
    {
        // given
        var repaymentSystemPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = repaymentSystemPage.GetGdsSubmitButtonById("continue-button");

        // when
        var additionalProjectsPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "Refinance", FundingFormOption.Repay }, { "RefinanceInfo", string.Empty } });

        // then
        additionalProjectsPage
            .UrlEndWith(FundingPageUrls.AdditionalProjectsSuffix)
            .HasTitle(FundingPageTitles.AdditionalProjects);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldMoveToPrivateSectorFunding_WhenRefinanceIsSelectedAndAdditionalDataProvided()
    {
        // given
        var repaymentSystemPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = repaymentSystemPage.GetGdsSubmitButtonById("continue-button");

        // when
        var additionalProjectsPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "Refinance", FundingFormOption.Refinance }, { "RefinanceInfo", TextTestData.TextWithLenght1000 } });

        // then
        additionalProjectsPage
            .UrlEndWith(FundingPageUrls.AdditionalProjectsSuffix)
            .HasTitle(FundingPageTitles.AdditionalProjects);
    }
}
