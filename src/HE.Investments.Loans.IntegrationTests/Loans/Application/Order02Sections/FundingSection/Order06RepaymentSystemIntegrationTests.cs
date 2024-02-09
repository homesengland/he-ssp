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

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.FundingSection;

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
            continueButton, new Dictionary<string, string> { { "Refinance", FundingFormOption.Refinance }, { "RefinanceInfo", TextTestData.TextThatExceedsLongInputLimit } });

        // then
        repaymentSystemPage
            .UrlEndWith(FundingPageUrls.RepaymentSystemSuffix)
            .HasTitle(FundingPageTitles.RepaymentSystem)
            .ContainsValidationMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.RefinanceInfo));
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
            continueButton, new Dictionary<string, string> { { "Refinance", FundingFormOption.Refinance }, { "RefinanceInfo", TextTestData.TextThatNotExceedsLongInputLimit } });

        // then
        additionalProjectsPage
            .UrlEndWith(FundingPageUrls.AdditionalProjectsSuffix)
            .HasTitle(FundingPageTitles.AdditionalProjects);
    }
}
