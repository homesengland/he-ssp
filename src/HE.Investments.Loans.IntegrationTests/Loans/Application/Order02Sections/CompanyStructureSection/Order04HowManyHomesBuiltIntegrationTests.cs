using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.CompanyStructureSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order04HowManyHomesBuiltIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;

    public Order04HowManyHomesBuiltIntegrationTests(LoansIntegrationTestFixture fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenNotNumberIsProvided()
    {
        // given
        var howManyHomesBuiltPage =
            await TestClient.NavigateTo(CompanyStructurePagesUrls.HowManyHomesBuilt(UserOrganisationData.OrganisationId, _applicationLoanId));
        var continueButton = howManyHomesBuiltPage.GetGdsSubmitButtonById("continue-button");

        // when
        howManyHomesBuiltPage = await TestClient.SubmitButton(
            continueButton,
            new Dictionary<string, string> { { "HomesBuilt", "NotNumber" } });

        // then
        howManyHomesBuiltPage
            .UrlEndWith(CompanyStructurePagesUrls.HowManyHomesBuiltSuffix)
            .HasOneValidationMessages("The amount of homes your organisation has built must be a number");

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
            continueButton,
            new Dictionary<string, string> { { "HomesBuilt", "13" } });

        // then
        checkYourAnswersPage
            .UrlEndWith(CompanyStructurePagesUrls.CheckYourAnswersSuffix)
            .GetPageTitle()
            .Should()
            .EndWith("Check your answers");
    }
}
