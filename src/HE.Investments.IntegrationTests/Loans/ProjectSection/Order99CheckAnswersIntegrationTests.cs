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
using HE.InvestmentLoans.WWW.Views.Project.Consts;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order99CheckAnswersIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order99CheckAnswersIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayDataSummary()
    {
        var checkAnswersPage = await TestClient.NavigateTo(ProjectPagesUrls.CheckAnswers(_applicationLoanId, _projectId));

        var projectSummary = checkAnswersPage.GetSummaryListItems();

        using (new AssertionScope())
        {
            projectSummary[ProjectFieldNames.Name].Should().Be(TextTestData.TextThatNotExceedsShortInputLimit);
            projectSummary[ProjectFieldNames.PlanningReferenceNumberExists].Should().Be(CommonResponse.Yes);
            projectSummary[ProjectFieldNames.PlanningReferenceNumber].Should().Be(TextTestData.TextThatNotExceedsShortInputLimit);
            projectSummary[ProjectFieldNames.StartDate].Should().Be(TextTestData.TextThatNotExceedsShortInputLimit);
        }

        SetSharedData(SharedKeys.CurrentPageKey, checkAnswersPage);
    }
}
