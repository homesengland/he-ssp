using System.Diagnostics.CodeAnalysis;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.Project.Consts;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.ProjectSection;

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
            continueButton, new Dictionary<string, string> { { "ProjectName", TextTestData.TextThatExceedsShortInputLimit } });

        // then
        projectNamePage
            .UrlEndWith(ProjectPagesUrls.NameSuffix)
            .HasTitle(ProjectPageTitles.Name)
            .HasOneValidationMessages(ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.ProjectName));

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
            continueButton, new Dictionary<string, string> { { "ProjectName", TextTestData.TextThatNotExceedsShortInputLimit } });

        // then
        startDatePage
            .UrlEndWith(ProjectPagesUrls.StartDateSuffix)
            .HasTitle(ProjectPageTitles.StartDate);
    }
}
