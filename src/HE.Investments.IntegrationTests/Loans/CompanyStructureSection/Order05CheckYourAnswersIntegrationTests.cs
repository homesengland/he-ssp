using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.TestData;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.CompanyStructureSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order05CheckYourAnswersIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;

    public Order05CheckYourAnswersIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = GetSharedData<string>(SharedKeys.ApplicationLoanIdInDraftStatusKey);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayDataSummary()
    {
        // given
        var checkYourAnswersPage = await TestClient.NavigateTo(CompanyStructurePagesUrls.CheckYourAnswers(_applicationLoanId));

        // when
        var companyStructureSummary = checkYourAnswersPage.GetSummaryListItems();

        // then
        companyStructureSummary[CompanyStructureFields.CompanyEstablishedForThisDevelopment].Should().Be(CommonResponse.Yes);
        companyStructureSummary[CompanyStructureFields.AdditionalInformation].Should().Be(TextTestData.TextWithLenght1000);
        companyStructureSummary[CompanyStructureFields.HomesInLastThreeYears].Should().Be("13");
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
            .UrlEndWith(CompanyStructurePagesUrls.CheckYourAnswersSuffix)
            .HasOneValidationMessages("Select whether you have completed this section");

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
            .UrlEndWith(ApplicationPagesUrls.TaskList(_applicationLoanId))
            .GetTaskListItems()[TaskListFields.CompleteCompanyInformation].Should().Be("Completed");
    }
}
