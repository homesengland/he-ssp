using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using FluentAssertions;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.Security.Consts;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.SecuritySection;
[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order05CheckYourAnswersIntegrationTests : IntegrationTest
{
    private readonly string _applicationId;

    public Order05CheckYourAnswersIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationId = UserData.LoanApplicationIdInDraftState;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayDataSummary()
    {
        // given
        var checkYourAnswersPage = await TestClient.NavigateTo(SecurityPageUrls.CheckYourAnswers(_applicationId));

        // when
        var companyStructureSummary = checkYourAnswersPage.GetSummaryListItems();

        // then
        companyStructureSummary[SecurityFields.ChargesDebt].Should().Be(CommonResponse.Yes);
        companyStructureSummary[SecurityFields.Debenture].Should().Contain(TextTestData.TextThatNotExceedsLongInputLimit);
        companyStructureSummary[SecurityFields.DirLoans].Should().Be(CommonResponse.Yes);
        companyStructureSummary[SecurityFields.DirLoansSub].Should().Contain(CommonResponse.No).And.Contain(TextTestData.TextThatNotExceedsLongInputLimit);

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
            .UrlEndWith(SecurityPageUrls.CheckYourAnswersSuffix)
            .HasOneValidationMessages("Select whether you have completed this section");
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldRedirectToChargesDebt_WhenChargesDebtChangeClicked()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("charges-debt-change");

        // when
        var chargesDebtPage = await TestClient.NavigateTo(changeLink);

        // then
        chargesDebtPage
            .UrlWithoutQueryEndsWith(SecurityPageUrls.ChargesDebtSuffix)
            .HasTitle(SecurityPageTitles.ChargesDebt);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldGoBackToCheckAnswers_WhenChangingChargesDebt()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("debenture-change");
        var chargesDebtPage = await TestClient.NavigateTo(changeLink);

        // when
        var continueButton = chargesDebtPage.GetGdsSubmitButtonById("continue-button");
        var returnPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "ChargesDebtCompany", CommonResponse.Yes }, { "ChargesDebtCompanyInfo", TextTestData.TextThatNotExceedsLongInputLimit } });

        // then
        returnPage
            .UrlEndWith(SecurityPageUrls.CheckYourAnswersSuffix)
            .HasTitle(SecurityPageTitles.CheckAnswers);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldRedirectToChargesDebt_WhenDebentureChangeClicked()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("debenture-change");

        // when
        var chargesDebtPage = await TestClient.NavigateTo(changeLink);

        // then
        chargesDebtPage
            .UrlWithoutQueryEndsWith(SecurityPageUrls.ChargesDebtSuffix)
            .HasTitle(SecurityPageTitles.ChargesDebt);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldRedirectToDirLoans_WhenDirLoansChangeClicked()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("dir-loans-change");

        // when
        var dirLoansPage = await TestClient.NavigateTo(changeLink);

        // then
        dirLoansPage
            .UrlWithoutQueryEndsWith(SecurityPageUrls.DirectorLoansSuffix)
            .HasTitle(SecurityPageTitles.DirLoans);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ShouldGoBackToCheckAnswers_WhenChangingDirLoans()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("dir-loans-change");
        var chargesDebtPage = await TestClient.NavigateTo(changeLink);

        // when
        var continueButton = chargesDebtPage.GetGdsSubmitButtonById("continue-button");
        var returnPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "DirLoans", CommonResponse.No } });

        // then
        returnPage
            .UrlEndWith(SecurityPageUrls.CheckYourAnswersSuffix)
            .HasTitle(SecurityPageTitles.CheckAnswers);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ShouldGoDirLoansSub_WhenChangingDirLoansAndTheValueIsYes()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("dir-loans-change");
        var chargesDebtPage = await TestClient.NavigateTo(changeLink);

        // when
        var continueButton = chargesDebtPage.GetGdsSubmitButtonById("continue-button");
        var returnPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "DirLoans", CommonResponse.Yes } });

        // then
        returnPage
            .UrlWithoutQueryEndsWith(SecurityPageUrls.DirLoansSubSuffix)
            .HasTitle(SecurityPageTitles.DirLoansSub);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(9)]
    public async Task Order09_ShouldRedirectToDirLoansSub_WhenDirLoansSubChangeClicked()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("dir-loans-sub-change");

        // when
        var dirLoansSubPage = await TestClient.NavigateTo(changeLink);

        // then
        dirLoansSubPage
            .UrlWithoutQueryEndsWith(SecurityPageUrls.DirLoansSubSuffix)
            .HasTitle(SecurityPageTitles.DirLoansSub);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(10)]
    public async Task Order10_ShouldGoBackToCheckAnswers_WhenChangingDirLoansSub()
    {
        // given
        var checkYourAnswersPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var changeLink = checkYourAnswersPage.GetAnchorElementById("dir-loans-sub-change");
        var dirLoansSubPage = await TestClient.NavigateTo(changeLink);

        // when
        var continueButton = dirLoansSubPage.GetGdsSubmitButtonById("continue-button");
        var returnPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "DirLoansSub", CommonResponse.No }, { "DirLoansSubMore", TextTestData.TextThatNotExceedsLongInputLimit } });

        // then
        returnPage
            .UrlEndWith(SecurityPageUrls.CheckYourAnswersSuffix)
            .HasTitle(SecurityPageTitles.CheckAnswers);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(11)]
    public async Task Order11_ShouldCompletedSection_WhenYesAnswerIsSelected()
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
            .GetTaskListItems()[TaskListFields.ProvideDetailsAboutSecurity].Should().Be("Completed");
    }
}
