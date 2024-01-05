using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.CompanyStructureSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order05CheckYourAnswersIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;

    public Order05CheckYourAnswersIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
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
        companyStructureSummary[CompanyStructureFields.AdditionalInformation].Should().Be(TextTestData.TextThatNotExceedsLongInputLimit);
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
