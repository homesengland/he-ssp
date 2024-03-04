using System.Diagnostics.CodeAnalysis;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.Messages;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.Project.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.ProjectSection;

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
            continueButton, new Dictionary<string, string> { { "HasEstimatedStartDate", CommonResponse.Yes }, { "EstimatedStartDay", string.Empty }, { "EstimatedStartMonth", string.Empty }, { "EstimatedStartYear", string.Empty } });

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

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldRedirectToManyHomes_WhenYesIsSelectedAndValidStartDateIsProvided()
    {
        // given
        var startDate = await GetCurrentPage(() => TestClient.NavigateTo(ProjectPagesUrls.StartDate(_applicationLoanId, _projectId)));
        var continueButton = startDate.GetGdsSubmitButtonById("continue-button");

        var (year, month, day) = DateTimeTestData.CorrectDateAsStrings;

        // when
        startDate = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "HasEstimatedStartDate", CommonResponse.Yes }, { "EstimatedStartDay", day }, { "EstimatedStartMonth", month }, { "EstimatedStartYear", year } });

        // then
        startDate
            .UrlEndWith(ProjectPagesUrls.ManyHomesSuffix)
            .HasLabelTitle(ProjectPageTitles.ManyHomes);
    }
}
