using AngleSharp.Html.Dom;
using FluentAssertions;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.TestData;
using HE.InvestmentLoans.IntegrationTests.Loans.CompanyStructureSection;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.SecuritySection;
internal class Order05CheckYourAnswersIntegrationTests : IntegrationTest
{
    private readonly string _applicationId;

    public Order05CheckYourAnswersIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationId = GetSharedData<string>(SharedKeys.ApplicationLoanIdInDraftStatusKey);
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
        companyStructureSummary[SecurityFields.DirLoans].Should().Be(CommonResponse.Yes);
        companyStructureSummary[SecurityFields.DirLoansSub].Should().Contain(CommonResponse.No).And.Contain(TextTestData.TextWithLenght1000);

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
            .HasValidationMessages("Select whether you have completed this section");

        SetSharedData(SharedKeys.CurrentPageKey, checkYourAnswersPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldCompletedSection_WhenYesAnswerIsSelected()
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
