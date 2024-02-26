using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.Common.Extensions;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.FundingV2.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;
using CommonResponse = HE.Investments.Loans.Common.Utils.Constants.FormOption.CommonResponse;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.FundingSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order08CheckYourAnswersIntegrationTests : IntegrationTest
{
    private readonly string _applicationId;

    public Order08CheckYourAnswersIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationId = UserData.LoanApplicationIdInDraftState;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayDataSummary()
    {
        // given
        var checkYourAnswersPage = await TestClient.NavigateTo(FundingPageUrls.CheckYourAnswers(_applicationId));

        // when
        var fundingSummary = checkYourAnswersPage.GetSummaryListItems();

        // then
        fundingSummary[FundingFields.GrossDevelopmentValue].Should().Be("£12");
        fundingSummary[FundingFields.EstimatedTotalCosts].Should().Be("£999");
        fundingSummary[FundingFields.AbnormalCosts].Should().Contain(CommonResponse.Yes).And.Contain(TextTestData.TextThatNotExceedsLongInputLimit);
        fundingSummary[FundingFields.PrivateSectorFunding].Should().Contain(CommonResponse.Yes).And.Contain(TextTestData.TextThatNotExceedsLongInputLimit);
        fundingSummary[FundingFields.RefinanceOrRepay].Should().Contain(FundingFormOption.Refinance.TitleCaseFirstLetterInString()).And.Contain(TextTestData.TextThatNotExceedsLongInputLimit);
        fundingSummary[FundingFields.AdditionalProjects].Should().Be(CommonResponse.No);

        SetSharedData(SharedKeys.CurrentPageKey, checkYourAnswersPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldDisplayValidationError_WhenNoAnswersAreSelected()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = checkYourAnswersPage.GetGdsSubmitButtonById("continue-button");

        // when
        checkYourAnswersPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "CheckAnswers", string.Empty } });

        // then
        checkYourAnswersPage
            .UrlEndWith(FundingPageUrls.CheckYourAnswersSuffix)
            .HasValidationMessages("Select whether you have completed this section");
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldRedirectToGrossDevelopmentValue_WhenGrossDevelopmentValueClicked()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("gross-development-value-change");

        // when
        var grossDevelopmentValuePage = await TestClient.NavigateTo(changeLink);

        // then
        grossDevelopmentValuePage
            .UrlWithoutQueryEndsWith(FundingPageUrls.GrossDevelopmentValueSuffix)
            .HasTitle(FundingPageTitles.GrossDevelopmentValue);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldGoBackToCheckAnswers_WhenChangingGrossDevelopmentValue()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("gross-development-value-change");
        var grossDevelopmentValuePage = await TestClient.NavigateTo(changeLink);

        // when
        var continueButton = grossDevelopmentValuePage.GetGdsSubmitButtonById("continue-button");
        var returnPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "GrossDevelopmentValue", "12" } });

        // then
        returnPage
            .UrlEndWith(FundingPageUrls.CheckYourAnswersSuffix)
            .HasTitle(FundingPageTitles.CheckAnswers);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldRedirectToEstimatedTotalCosts_WhenEstimatedTotalCostsClicked()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("estimated-total-costs-change");

        // when
        var estimatedTotalCostsPage = await TestClient.NavigateTo(changeLink);

        // then
        estimatedTotalCostsPage
            .UrlWithoutQueryEndsWith(FundingPageUrls.EstimatedTotalCostsSuffix)
            .HasLabelTitle(FundingPageTitles.EstimatedTotalCosts);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldGoBackToCheckAnswers_WhenChangingEstimatedTotalCosts()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("estimated-total-costs-change");
        var estimatedTotalCostsPage = await TestClient.NavigateTo(changeLink);

        // when
        var continueButton = estimatedTotalCostsPage.GetGdsSubmitButtonById("continue-button");
        var returnPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "TotalCosts", "999" } });

        // then
        returnPage
            .UrlEndWith(FundingPageUrls.CheckYourAnswersSuffix)
            .HasTitle(FundingPageTitles.CheckAnswers);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ShouldRedirectToAbnormalCosts_WhenAbnormalCostsClicked()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("abnormal-costs-change");

        // when
        var abnormalCostsPage = await TestClient.NavigateTo(changeLink);

        // then
        abnormalCostsPage
            .UrlWithoutQueryEndsWith(FundingPageUrls.AbnormalCostsSuffix)
            .HasTitle(FundingPageTitles.AbnormalCosts);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ShouldGoBackToCheckAnswers_WhenChangingAbnormalCosts()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("abnormal-costs-change");
        var abnormalCostsPage = await TestClient.NavigateTo(changeLink);

        // when
        var continueButton = abnormalCostsPage.GetGdsSubmitButtonById("continue-button");
        var returnPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "AbnormalCosts", CommonResponse.Yes }, { "AbnormalCostsInfo", TextTestData.TextThatNotExceedsLongInputLimit } });

        // then
        returnPage
            .UrlEndWith(FundingPageUrls.CheckYourAnswersSuffix)
            .HasTitle(FundingPageTitles.CheckAnswers);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(9)]
    public async Task Order09_ShouldRedirectToPrivateSectorFunding_WhenPrivateSectorFundingClicked()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("private-sector-funding-change");

        // when
        var privateSectorFundingPage = await TestClient.NavigateTo(changeLink);

        // then
        privateSectorFundingPage
            .UrlWithoutQueryEndsWith(FundingPageUrls.PrivateSectorFundingSuffix)
            .HasTitle(FundingPageTitles.PrivateSectorFunding);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(10)]
    public async Task Order10_ShouldGoBackToCheckAnswers_WhenChangingPrivateSectorFunding()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("private-sector-funding-change");
        var privateSectorFundingPage = await TestClient.NavigateTo(changeLink);

        // when
        var continueButton = privateSectorFundingPage.GetGdsSubmitButtonById("continue-button");
        var returnPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "PrivateSectorFunding", CommonResponse.Yes }, { "PrivateSectorFundingResult", TextTestData.TextThatNotExceedsLongInputLimit }, { "PrivateSectorFundingReason", string.Empty } });

        // then
        returnPage
            .UrlEndWith(FundingPageUrls.CheckYourAnswersSuffix)
            .HasTitle(FundingPageTitles.CheckAnswers);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(11)]
    public async Task Order11_ShouldRedirectToRepaymentSystem_WhenRepaymentSystemClicked()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("repayment-system-change");

        // when
        var repaymentSystemPage = await TestClient.NavigateTo(changeLink);

        // then
        repaymentSystemPage
            .UrlWithoutQueryEndsWith(FundingPageUrls.RepaymentSystemSuffix)
            .HasTitle(FundingPageTitles.RepaymentSystem);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(12)]
    public async Task Order12_ShouldGoBackToCheckAnswers_WhenChangingRepaymentSystem()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("repayment-system-change");
        var repaymentSystemPage = await TestClient.NavigateTo(changeLink);

        // when
        var continueButton = repaymentSystemPage.GetGdsSubmitButtonById("continue-button");
        var returnPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "Refinance", FundingFormOption.Refinance }, { "RefinanceInfo", TextTestData.TextThatNotExceedsLongInputLimit } });

        // then
        returnPage
            .UrlEndWith(FundingPageUrls.CheckYourAnswersSuffix)
            .HasTitle(FundingPageTitles.CheckAnswers);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(13)]
    public async Task Order13_ShouldRedirectToAdditionalProjects_WhenAdditionalProjectsClicked()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("additional-projects-change");

        // when
        var additionalProjectsPage = await TestClient.NavigateTo(changeLink);

        // then
        additionalProjectsPage
            .UrlWithoutQueryEndsWith(FundingPageUrls.AdditionalProjectsSuffix)
            .HasTitle(FundingPageTitles.AdditionalProjects);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(14)]
    public async Task Order14_ShouldGoBackToCheckAnswers_WhenChangingAdditionalProjects()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("additional-projects-change");
        var additionalProjectsPage = await TestClient.NavigateTo(changeLink);

        // when
        var continueButton = additionalProjectsPage.GetGdsSubmitButtonById("continue-button");
        var returnPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "AdditionalProjects", CommonResponse.No } });

        // then
        returnPage
            .UrlEndWith(FundingPageUrls.CheckYourAnswersSuffix)
            .HasTitle(FundingPageTitles.CheckAnswers);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(15)]
    public async Task Order15_ShouldCompletedSection_WhenYesAnswerIsSelected()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = checkYourAnswersPage.GetGdsSubmitButtonById("continue-button");

        // when
        var taskListPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "CheckAnswers", CommonResponse.Yes } });

        // then
        taskListPage
            .UrlEndWith(ApplicationPagesUrls.TaskList(_applicationId))
            .GetTaskListItems()[TaskListFields.ProvideDetailsAboutFunding].Should().Be("Completed");
    }
}
