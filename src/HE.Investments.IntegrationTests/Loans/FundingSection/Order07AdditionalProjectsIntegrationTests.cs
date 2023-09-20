using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.InvestmentLoans.WWW.Views.FundingV2.Consts;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.FundingSection;

[Order(7)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order07AdditionalProjectsIntegrationTests : IntegrationTest
{
    public Order07AdditionalProjectsIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldMoveToAdditionalProjects_WhenYesIsSelected()
    {
        // given
        var additionalProjectsPage = await TestClient.NavigateTo(FundingPageUrls.AdditionalProjects(GetSharedData<string>(SharedKeys.ApplicationLoanIdInDraftStatusKey)));
        var continueButton = additionalProjectsPage.GetGdsSubmitButtonById("continue-button");

        // when
        var checkAnswersPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "AdditionalProjects", CommonResponse.Yes } });

        // then
        checkAnswersPage
            .UrlEndWith(FundingPageUrls.CheckYourAnswersSuffix)
            .HasTitle(FundingPageTitles.CheckAnswers);

        SetSharedData(SharedKeys.CurrentPageKey, additionalProjectsPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldMoveToAdditionalProjects_WhenNoIsSelected()
    {
        // given
        var additionalProjectsPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = additionalProjectsPage.GetGdsSubmitButtonById("continue-button");

        // when
        var checkAnswersPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "AdditionalProjects", CommonResponse.No } });

        // then
        checkAnswersPage
            .UrlEndWith(FundingPageUrls.CheckYourAnswersSuffix)
            .HasTitle(FundingPageTitles.CheckAnswers);
    }
}
