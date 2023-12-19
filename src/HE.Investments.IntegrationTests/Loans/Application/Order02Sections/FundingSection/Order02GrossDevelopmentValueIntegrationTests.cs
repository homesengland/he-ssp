using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.Common.Messages;
using HE.Investments.IntegrationTestsFramework.Extensions;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.FundingV2.Consts;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.FundingSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order02GrossDevelopmentValueIntegrationTests : IntegrationTest
{
    public Order02GrossDevelopmentValueIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenProvidedValueIsNotADecimalNumber()
    {
        // given
        var grossDevelopmentValuePage = await TestClient.NavigateTo(FundingPageUrls.GrossDevelopmentValue(UserData.LoanApplicationIdInDraftState));
        var continueButton = grossDevelopmentValuePage.GetGdsSubmitButtonById("continue-button");

        // when
        grossDevelopmentValuePage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "GrossDevelopmentValue", "random string" } });

        // then
        grossDevelopmentValuePage
            .UrlEndWith(FundingPageUrls.GrossDevelopmentValueSuffix)
            .HasLabelTitle(FundingPageTitles.GrossDevelopmentValue)
            .ContainsValidationMessage(ValidationErrorMessage.EstimatedPoundInput("GDV"));

        SetSharedData(SharedKeys.CurrentPageKey, grossDevelopmentValuePage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldMoveToEstimatedTotalCosts_WhenDecimalValueIsProvided()
    {
        // given
        var grossDevelopmentValuePage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = grossDevelopmentValuePage.GetGdsSubmitButtonById("continue-button");

        // when
        var estimatedTotalCostsPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "GrossDevelopmentValue", "12" } });

        // then
        estimatedTotalCostsPage
            .UrlEndWith(FundingPageUrls.EstimatedTotalCostsSuffix)
            .HasLabelTitle(FundingPageTitles.EstimatedTotalCosts);
    }
}
