using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.InvestmentLoans.IntegrationTests.Loans.SecuritySection;
using HE.InvestmentLoans.WWW.Extensions;
using HE.InvestmentLoans.WWW.Helpers;
using HE.InvestmentLoans.WWW.Models;
using HE.InvestmentLoans.WWW.Views.Project.Consts;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order98CheckAnswersIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order98CheckAnswersIntegrationTests(IntegrationTestFixture<Program> fixture)
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
        projectSummary[ProjectFieldNames.Name].Should().Be(TextTestData.TextThatNotExceedsShortInputLimit);
        projectSummary[ProjectFieldNames.StartDate].Should().Be(TextTestData.TextThatNotExceedsShortInputLimit);
        projectSummary[ProjectFieldNames.PlanningReferenceNumberExists].Should().Be(CommonResponse.Yes);
        projectSummary[ProjectFieldNames.PlanningReferenceNumber].Should().Be(TextTestData.TextThatNotExceedsShortInputLimit);

        var statusDisplay = TemporaryFormOptions.PermissionStatus.GetSummaryLabel(ProjectFormOption.PlanningPermissionStatusOptions.NotSubmitted);
        projectSummary[ProjectFieldNames.PlanningPermissionStatus].Should().Be(statusDisplay);

        projectSummary[ProjectFieldNames.LandRegistryTitleNumber].Should().Be(TextTestData.TextThatNotExceedsLongInputLimit);
        projectSummary[ProjectFieldNames.LandRegistryTitleNumber].Should().Be(TextTestData.TextThatNotExceedsLongInputLimit);

        projectSummary.Should().NotContainKey(ProjectFieldNames.Coordinates);

        projectSummary[ProjectFieldNames.LandOwnership].Should().Be(CommonResponse.Yes);

        CheckIfAdditionalDetailsAreCorrect(projectSummary);

        projectSummary[ProjectFieldNames.GrantFundingExists].Should().Be(CommonResponse.Yes);
        CheckIfGrantFundingInformationIsCorrect(projectSummary);

        SetSharedData(SharedKeys.CurrentPageKey, checkAnswersPage);
    }

    private static void CheckIfAdditionalDetailsAreCorrect(IDictionary<string, string> projectSummary)
    {
        var additionalDetails = projectSummary[ProjectFieldNames.AdditionalDetails];

        additionalDetails.Should().Contain(DateTimeTestData.CorrectDateDisplay);
        additionalDetails.Should().Contain($"Purchase cost: {PoundsTestData.CorrectAmountDisplay}");
        additionalDetails.Should().Contain($"Current value: {PoundsTestData.CorrectAmountDisplay}");
        additionalDetails.Should().Contain(SourceOfValuationTestData.AnySourceDisplay);
    }

    private static void CheckIfGrantFundingInformationIsCorrect(IDictionary<string, string> projectSummary)
    {
        var additionalDetails = projectSummary[ProjectFieldNames.GrantFunding];

        additionalDetails.Should().Contain($"Previous funding: {TextTestData.TextThatNotExceedsShortInputLimit}");
        additionalDetails.Should().Contain($"Amount: {PoundsTestData.CorrectAmountDisplay}");
        additionalDetails.Should().Contain($"Name of the grant/found: {TextTestData.TextThatNotExceedsShortInputLimit}");
        additionalDetails.Should().Contain($"It was for: {TextTestData.TextThatNotExceedsLongInputLimit}");
    }
}
