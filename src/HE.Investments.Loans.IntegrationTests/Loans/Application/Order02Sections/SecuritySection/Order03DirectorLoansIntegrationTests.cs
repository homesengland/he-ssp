using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.Security.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.SecuritySection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order03DirectorLoansIntegrationTests : IntegrationTest
{
    private readonly string _applicationId;

    public Order03DirectorLoansIntegrationTests(LoansIntegrationTestFixture fixture)
        : base(fixture)
    {
        _applicationId = UserData.LoanApplicationIdInDraftState;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldMoveToCheckAnswers_WhenNoIsSelectedAndContinueButtonIsClicked()
    {
        // given
        var dirLoansPage = await TestClient.NavigateTo(SecurityPageUrls.DirectorLoans(_applicationId));
        var continueButton = dirLoansPage.GetGdsSubmitButtonById("continue-button");

        // when
        var checkAnswersPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "DirLoans", CommonResponse.No } });

        // then
        checkAnswersPage
            .UrlEndWith(SecurityPageUrls.CheckYourAnswersSuffix)
            .HasTitle(SecurityPageTitles.CheckAnswers);

        SetSharedData(SharedKeys.CurrentPageKey, dirLoansPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldMoveToCheckAnswers_WhenNeitherOptionIsSelectedAndContinueButtonIsClicked()
    {
        // given
        var dirLoansPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = dirLoansPage.GetGdsSubmitButtonById("continue-button");

        // when
        var checkAnswersPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "DirLoans", string.Empty } });

        // then
        checkAnswersPage
            .UrlEndWith(SecurityPageUrls.CheckYourAnswersSuffix)
            .HasTitle(SecurityPageTitles.CheckAnswers);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldMoveToDirLoansSub_WhenYesIsSelectedAndContinueButtonIsClicked()
    {
        // given
        var dirLoansPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = dirLoansPage.GetGdsSubmitButtonById("continue-button");

        // when
        var dirLoansSubPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "DirLoans", CommonResponse.Yes } });

        // then
        dirLoansSubPage
            .UrlEndWith(SecurityPageUrls.DirLoansSubSuffix)
            .HasTitle(SecurityPageTitles.DirLoansSub);
    }
}
