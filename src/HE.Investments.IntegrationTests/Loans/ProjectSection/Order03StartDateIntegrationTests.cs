using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.InvestmentLoans.WWW.Views.Project.Consts;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order03StartDateIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order03StartDateIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenYesIsSelectedButStartDateIsNotProvided()
    {
        // given
        var startDatePage = await TestClient.NavigateTo(ProjectPagesUrls.StartDate(_applicationLoanId, _projectId));
        var continueButton = startDatePage.GetGdsSubmitButtonById("continue-button");

        // when
        startDatePage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "HasEstimatedStartDate", CommonResponse.Yes } });

        // then
        startDatePage
            .UrlEndWith(ProjectPagesUrls.StartDateSuffix)
            .HasTitle(ProjectPageTitles.StartDate)
            .HasOneValidationMessages(ValidationErrorMessage.NoStartDate);

        SetSharedData(SharedKeys.CurrentPageKey, startDatePage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldDisplayValidationError_WhenYesIsSelectedButInvalidStartDateIsProvided()
    {
        // given
        var startDate = await GetCurrentPage(() => TestClient.NavigateTo(ProjectPagesUrls.StartDate(_applicationLoanId, _projectId)));
        var continueButton = startDate.GetGdsSubmitButtonById("continue-button");

        // when
        startDate = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "HasEstimatedStartDate", CommonResponse.Yes }, { "EstimatedStartDay", "32" }, { "EstimatedStartMonth", "1" }, { "EstimatedStartYear", "2020" } });

        // then
        startDate
            .UrlEndWith(ProjectPagesUrls.StartDateSuffix)
            .HasTitle(ProjectPageTitles.StartDate)
            .HasOneValidationMessages(ValidationErrorMessage.InvalidStartDate);
    }

    [Fact(Skip = "Many homes view not implemented yet")]
    [Order(3)]
    public async Task Order03_ShouldRedirectToManyHomes_WhenYesIsSelectedAndValidStartDateIsProvided()
    {
        // given
        var startDate = await GetCurrentPage(() => TestClient.NavigateTo(ProjectPagesUrls.StartDate(_applicationLoanId, _projectId)));
        var continueButton = startDate.GetGdsSubmitButtonById("continue-button");

        // when
        startDate = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "HasEstimatedStartDate", CommonResponse.Yes }, { "EstimatedStartDay", "31" }, { "EstimatedStartMonth", "1" }, { "EstimatedStartYear", "2020" } });

        // then
        startDate
            .UrlEndWith(ProjectPagesUrls.ManyHomesSuffix)
            .HasTitle(ProjectPageTitles.StartDate);
    }
}
