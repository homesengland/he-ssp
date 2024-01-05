using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.Common.Messages;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
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
public class Order04AbnormalCostsIntegrationTests : IntegrationTest
{
    public Order04AbnormalCostsIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenYesIsSelectedAndAdditionalInformationIsNotProvided()
    {
        // given
        var abnormalCostsPage = await TestClient.NavigateTo(FundingPageUrls.AbnormalCosts(UserData.LoanApplicationIdInDraftState));
        var continueButton = abnormalCostsPage.GetGdsSubmitButtonById("continue-button");

        // when
        abnormalCostsPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "AbnormalCosts", CommonResponse.Yes }, { "AbnormalCostsInfo", string.Empty } });

        // then
        abnormalCostsPage
            .UrlEndWith(FundingPageUrls.AbnormalCostsSuffix)
            .HasTitle(FundingPageTitles.AbnormalCosts)
            .ContainsValidationMessage(ValidationErrorMessage.EnterMoreDetails);

        SetSharedData(SharedKeys.CurrentPageKey, abnormalCostsPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldDisplayValidationError_WhenYesIsSelectedAndAdditionalInformationIsLongerThan1500Characters()
    {
        // given
        var abnormalCostsPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);

        var continueButton = abnormalCostsPage.GetGdsSubmitButtonById("continue-button");

        // when
        abnormalCostsPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "AbnormalCosts", CommonResponse.Yes }, { "AbnormalCostsInfo", TextTestData.TextThatExceedsLongInputLimit } });

        // then
        abnormalCostsPage
            .UrlEndWith(FundingPageUrls.AbnormalCostsSuffix)
            .HasTitle(FundingPageTitles.AbnormalCosts)
            .ContainsValidationMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.AbnormalCostsInfo));
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldMoveToPrivateSectorFunding_WhenNoIsSelected()
    {
        // given
        var abnormalCostsPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = abnormalCostsPage.GetGdsSubmitButtonById("continue-button");

        // when
        var privateSectorFundingPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "AbnormalCosts", CommonResponse.No }, { "AbnormalCostsInfo", string.Empty } });

        // then
        privateSectorFundingPage
            .UrlEndWith(FundingPageUrls.PrivateSectorFundingSuffix)
            .HasTitle(FundingPageTitles.PrivateSectorFunding);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldMoveToPrivateSectorFunding_WhenYesIsSelectedAndAdditionalDataProvided()
    {
        // given
        var abnormalCostsPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = abnormalCostsPage.GetGdsSubmitButtonById("continue-button");

        // when
        var privateSectorFundingPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "AbnormalCosts", CommonResponse.Yes }, { "AbnormalCostsInfo", TextTestData.TextThatNotExceedsLongInputLimit } });

        // then
        privateSectorFundingPage
            .UrlEndWith(FundingPageUrls.PrivateSectorFundingSuffix)
            .HasTitle(FundingPageTitles.PrivateSectorFunding);
    }
}
