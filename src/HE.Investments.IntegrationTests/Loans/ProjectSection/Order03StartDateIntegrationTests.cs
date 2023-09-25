using System.Diagnostics.CodeAnalysis;
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

    protected Order03StartDateIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = GetSharedData<string>(SharedKeys.ApplicationLoanIdInDraftStatusKey);
        _projectId = GetSharedData<string>(SharedKeys.ProjectIdInDraftStatusKey);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenYesIsSelectedButStartDateIsNotProvided()
    {
        // given
        var startDate = await TestClient.NavigateTo(ProjectPagesUrls.Name(_applicationLoanId, _projectId));
        var continueButton = startDate.GetGdsSubmitButtonById("continue-button");

        // when
        startDate = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "HasEstimatedStartDate", CommonResponse.Yes } });

        // then
        startDate
            .UrlEndWith(ProjectPagesUrls.StartDateSuffix)
            .HasTitle(ProjectPageTitles.StartDate)
            .HasValidationMessages(ValidationErrorMessage.NoStartDate);

        SetSharedData(SharedKeys.CurrentPageKey, startDate);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order02_ShouldDisplayValidationError_WhenYesIsSelectedButInvalidStartDateIsProvided()
    {
        // given
        var startDate = await GetCurrentPage(() => TestClient.NavigateTo(ProjectPagesUrls.Name(_applicationLoanId, _projectId)));
        var continueButton = startDate.GetGdsSubmitButtonById("continue-button");

        // when
        startDate = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "HasEstimatedStartDate", CommonResponse.Yes }, { "EstimatedStartDay", "31" }, { "EstimatedStartMonth", "1" }, { "EstimatedStartYear", "2030" } });

        // then
        startDate
            .UrlEndWith(ProjectPagesUrls.StartDateSuffix)
            .HasTitle(ProjectPageTitles.StartDate)
            .HasValidationMessages(ValidationErrorMessage.InvalidStartDate);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order03_ShouldRedirectToManyHomes_WhenYesIsSelectedAndValidStartDateIsProvided()
    {
        // given
        var startDate = await GetCurrentPage(() => TestClient.NavigateTo(ProjectPagesUrls.Name(_applicationLoanId, _projectId)));
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
