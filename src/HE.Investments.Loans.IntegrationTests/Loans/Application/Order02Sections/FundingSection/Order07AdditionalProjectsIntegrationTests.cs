using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.IntegrationTestsFramework;
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
public class Order07AdditionalProjectsIntegrationTests : IntegrationTest
{
    public Order07AdditionalProjectsIntegrationTests(LoansIntegrationTestFixture fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldMoveToAdditionalProjects_WhenYesIsSelected()
    {
        // given
        var additionalProjectsPage = await TestClient.NavigateTo(FundingPageUrls.AdditionalProjects(UserData.LoanApplicationIdInDraftState));
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
