using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.InvestmentLoans.WWW.Views.FundingV2.Consts;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.FundingSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order03EstimatedTotalCostsIntegrationTests : IntegrationTest
{
    public Order03EstimatedTotalCostsIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenProvidedValueIsNotADecimalNumber()
    {
        // given
        var estimatedTotalCostsPage = await TestClient.NavigateTo(FundingPageUrls.EstimatedTotalCosts(UserData.LoanApplicationIdInDraftState));
        var continueButton = estimatedTotalCostsPage.GetGdsSubmitButtonById("continue-button");

        // when
        estimatedTotalCostsPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "TotalCosts", "random string" } });

        // then
        estimatedTotalCostsPage
            .UrlEndWith(FundingPageUrls.EstimatedTotalCostsSuffix)
            .HasLabelTitle(FundingPageTitles.EstimatedTotalCosts)
            .ContainsValidationMessage(ValidationErrorMessage.EstimatedPoundInput("total cost"));

        SetSharedData(SharedKeys.CurrentPageKey, estimatedTotalCostsPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldMoveToEstimatedTotalCosts_WhenDecimalValueIsProvided()
    {
        // given
        var estimatedTotalCostsPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = estimatedTotalCostsPage.GetGdsSubmitButtonById("continue-button");

        // when
        var abnormalCostsPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "TotalCosts", "999" } });

        // then
        abnormalCostsPage
            .UrlEndWith(FundingPageUrls.AbnormalCostsSuffix)
            .HasTitle(FundingPageTitles.AbnormalCosts);
    }
}
