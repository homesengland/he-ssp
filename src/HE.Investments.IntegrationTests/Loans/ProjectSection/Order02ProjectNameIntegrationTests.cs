using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
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
public class Order02ProjectNameIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order02ProjectNameIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenNameIsTooLong()
    {
        // given
        var projectNamePage = await TestClient.NavigateTo(ProjectPagesUrls.Name(_applicationLoanId, _projectId));
        var continueButton = projectNamePage.GetGdsSubmitButtonById("continue-button");

        // when
        projectNamePage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "Name", TextTestData.TextThatExceedsShortInputLimit } });

        // then
        projectNamePage
            .UrlEndWith(ProjectPagesUrls.NameSuffix)
            .HasTitle(ProjectPageTitles.Name)
            .HasValidationMessages(ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.ProjectName));

        SetSharedData(SharedKeys.CurrentPageKey, projectNamePage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldRedirectToStartDatePage_WhenProvidedNameIsCorrect()
    {
        // given
        var projectNamePage = await GetCurrentPage(() => TestClient.NavigateTo(ProjectPagesUrls.Name(_applicationLoanId, _projectId)));

        var continueButton = projectNamePage.GetGdsSubmitButtonById("continue-button");

        // when
        var startDatePage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "Name", TextTestData.TextThatNotExceedsShortInputLimit } });

        // then
        startDatePage
            .UrlEndWith(ProjectPagesUrls.StartDateSuffix)
            .HasTitle(ProjectPageTitles.StartDate);
    }
}
