using He.AspNetCore.Mvc.Gds.Components.Constants;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.SecuritySection;

[Order(2)]
public class Order03DirectorLoansIntegrationTests : IntegrationTest
{
    private readonly string _applicationId;

    public Order03DirectorLoansIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationId = GetSharedData<string>(SharedKeys.ApplicationLoanIdInDraftStatusKey);
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
            .HasTitle("Check your answers");
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldMoveToCheckAnswers_WhenNeitherOptionIsSelectedAndContinueButtonIsClicked()
    {
        // given
        var dirLoansPage = await TestClient.NavigateTo(SecurityPageUrls.DirectorLoans(_applicationId));
        var continueButton = dirLoansPage.GetGdsSubmitButtonById("continue-button");

        // when
        var checkAnswersPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "DirLoans", string.Empty } });

        // then
        checkAnswersPage
            .UrlEndWith(SecurityPageUrls.CheckYourAnswersSuffix)
            .HasTitle("Check your answers");
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldMoveToDirLoansSub_WhenYesIsSelectedAndContinueButtonIsClicked()
    {
        // given
        var dirLoansPage = await TestClient.NavigateTo(SecurityPageUrls.DirectorLoans(_applicationId));
        var continueButton = dirLoansPage.GetGdsSubmitButtonById("continue-button");

        // when
        var dirLoansSubPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "DirLoans", CommonResponse.Yes } });

        // then
        dirLoansSubPage
            .UrlEndWith(SecurityPageUrls.DirLoansSubSuffix)
            .HasTitle("Can you confirm these director loans can be subordinated?");
    }
}
