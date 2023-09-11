using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.CompanyStructureSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order04HowManyHomesBuiltIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;

    public Order04HowManyHomesBuiltIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = GetSharedData<string>(SharedKeys.ApplicationLoanIdInDraftStatusKey);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenNotNumberIsProvided()
    {
        // given
        var howManyHomesBuiltPage = await TestClient.NavigateTo(CompanyStructurePagesUrls.HowManyHomesBuilt(_applicationLoanId));
        var continueButton = howManyHomesBuiltPage.GetGdsSubmitButtonById("continue-button");

        // when
        howManyHomesBuiltPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "HomesBuilt", "NotNumber" } });

        // then
        howManyHomesBuiltPage.Url.Should().EndWith(CompanyStructurePagesUrls.HowManyHomesBuiltSuffix);
        howManyHomesBuiltPage.ContainsValidationMessage("The amount of homes your organisation has built must be a number");
        SetSharedData(SharedKeys.CurrentPageKey, howManyHomesBuiltPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldMoveToNextPageCheckYourAnswers_WhenCorrectNumberIsProvicedAndContinueButtonIsClicked()
    {
        // given
        var howManyHomesBuiltPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = howManyHomesBuiltPage.GetGdsSubmitButtonById("continue-button");

        // when
        var checkYourAnswersPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "HomesBuilt", "13" } });

        // then
        checkYourAnswersPage.Url.Should().EndWith(CompanyStructurePagesUrls.CheckYourAnswersSuffix);
        checkYourAnswersPage.GetPageTitle().Should().EndWith("Check your answers");
    }
}
