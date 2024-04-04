using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.Messages;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Extensions;
using HE.Investments.Loans.WWW.Models;
using HE.Investments.Loans.WWW.Views.Project.Consts;
using HE.Investments.TestsUtils.Extensions;
using HE.Investments.TestsUtils.Helpers;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order98CheckAnswersIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order98CheckAnswersIntegrationTests(LoansIntegrationTestFixture fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayDataSummary()
    {
        // given
        var checkAnswersPage = await TestClient.NavigateTo(ProjectPagesUrls.CheckAnswers(_applicationLoanId, _projectId));

        // when
        var projectSummary = checkAnswersPage.GetSummaryListItems();

        // then
        projectSummary[ProjectFieldNames.Name].Value.Should().Be(TextTestData.TextThatNotExceedsShortInputLimit);
        projectSummary[ProjectFieldNames.StartDate].Value.Should().Contain(DateTimeTestData.CorrectDateDisplay);
        projectSummary[ProjectFieldNames.ManyHomes].Value.Should().Contain("1");
        projectSummary[ProjectFieldNames.TypeHomes].Value.Should().Contain(TextTestData.TextThatNotExceedsShortInputLimit);
        projectSummary[ProjectFieldNames.ProjectType].Value.Should().Contain("Greenfield");
        projectSummary[ProjectFieldNames.PlanningReferenceNumberExists].Value.Should().Be(CommonResponse.Yes);
        projectSummary[ProjectFieldNames.PlanningReferenceNumber].Value.Should().Be(TextTestData.TextThatNotExceedsShortInputLimit);
        projectSummary[ProjectFieldNames.LocalAuthority].Value.Should().Be(UserData.LocalAuthorityName);

        var statusDisplay = FormOption.PermissionStatus.GetSummaryLabel(ProjectFormOption.PlanningPermissionStatusOptions.NotSubmitted);
        projectSummary[ProjectFieldNames.PlanningPermissionStatus].Value.Should().Be(statusDisplay);

        projectSummary[ProjectFieldNames.LandRegistryTitleNumber].Value.Should().Be(TextTestData.TextThatNotExceedsLongInputLimit);
        projectSummary[ProjectFieldNames.LandRegistryTitleNumber].Value.Should().Be(TextTestData.TextThatNotExceedsLongInputLimit);

        projectSummary.Should().NotContainKey(ProjectFieldNames.Coordinates);

        projectSummary[ProjectFieldNames.LandOwnership].Value.Should().Be(CommonResponse.Yes);

        CheckIfAdditionalDetailsAreCorrect(projectSummary);

        projectSummary[ProjectFieldNames.GrantFundingExists].Value.Should().Be(CommonResponse.Yes);
        CheckIfGrantFundingInformationIsCorrect(projectSummary);

        projectSummary[ProjectFieldNames.ChargesDebt].Value.Should().Contain(CommonResponse.Yes).And.Contain(TextTestData.TextThatNotExceedsLongInputLimit);
        projectSummary[ProjectFieldNames.AffordableHomes].Value.Should().Be(CommonResponse.Yes);

        SetCurrentPage(checkAnswersPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldDisplayValidationError_WhenNoAnswersAreSelected()
    {
        // given
        var checkYourAnswersPage = await GetCurrentPage(ProjectPagesUrls.CheckAnswers(_applicationLoanId, _projectId));
        var continueButton = checkYourAnswersPage.GetGdsSubmitButtonById("continue-button");

        // when
        checkYourAnswersPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "CheckAnswers", string.Empty } });

        // then
        checkYourAnswersPage
            .UrlEndWith(ProjectPagesUrls.CheckAnswersSuffix)
            .HasTitle(ProjectPageTitles.CheckAnswers)
            .HasOneValidationMessages(ValidationErrorMessage.NoCheckAnswers);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldCompletedSection_WhenYesAnswerIsSelected()
    {
        // given
        var checkYourAnswersPage = await GetCurrentPage(ProjectPagesUrls.CheckAnswers(_applicationLoanId, _projectId));
        var continueButton = checkYourAnswersPage.GetGdsSubmitButtonById("continue-button");

        // when
        var taskListPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "CheckAnswers", CommonResponse.Yes } });

        // then
        var (_, status, _) = taskListPage.GetProjectFromTaskList(_projectId);

        status.Should().Be("Completed");
    }

    private static void CheckIfAdditionalDetailsAreCorrect(IDictionary<string, SummaryItem> projectSummary)
    {
        var additionalDetails = projectSummary[ProjectFieldNames.AdditionalDetails].Value;

        additionalDetails.Should().Contain(DateTimeTestData.CorrectDateDisplay);
        additionalDetails.Should().Contain($"Purchase cost: {PoundsTestData.CorrectAmountDisplay}");
        additionalDetails.Should().Contain($"Current value: {PoundsTestData.CorrectAmountDisplay}");
        additionalDetails.Should().Contain(SourceOfValuationTestData.AnySourceDisplay);
    }

    private static void CheckIfGrantFundingInformationIsCorrect(IDictionary<string, SummaryItem> projectSummary)
    {
        var additionalDetails = projectSummary[ProjectFieldNames.GrantFunding].Value;

        additionalDetails.Should().Contain($"Previous funding: {TextTestData.TextThatNotExceedsShortInputLimit}");
        additionalDetails.Should().Contain($"Amount: {PoundsTestData.CorrectAmountDisplay}");
        additionalDetails.Should().Contain($"Name of the grant/found: {TextTestData.TextThatNotExceedsShortInputLimit}");
        additionalDetails.Should().Contain($"It was for: {TextTestData.TextThatNotExceedsLongInputLimit}");
    }
}
