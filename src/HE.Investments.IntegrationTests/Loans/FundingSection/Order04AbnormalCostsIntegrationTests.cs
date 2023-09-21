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
        var abnormalCostsPage = await TestClient.NavigateTo(FundingPageUrls.AbnormalCosts(GetSharedData<string>(SharedKeys.ApplicationLoanIdInDraftStatusKey)));
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
            continueButton, new Dictionary<string, string> { { "AbnormalCosts", CommonResponse.Yes }, { "AbnormalCostsInfo", TextTestData.TextWithLenght1501 } });

        // then
        abnormalCostsPage
            .UrlEndWith(FundingPageUrls.AbnormalCostsSuffix)
            .HasTitle(FundingPageTitles.AbnormalCosts)
            .ContainsValidationMessage(ValidationErrorMessage.LongInputLengthExceeded(FormOptions.FieldNameForInputLengthValidation.AbnormalCostsInfo));
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
            continueButton, new Dictionary<string, string> { { "AbnormalCosts", CommonResponse.Yes }, { "AbnormalCostsInfo", TextTestData.TextWithLenght1000 } });

        // then
        privateSectorFundingPage
            .UrlEndWith(FundingPageUrls.PrivateSectorFundingSuffix)
            .HasTitle(FundingPageTitles.PrivateSectorFunding);
    }
}
