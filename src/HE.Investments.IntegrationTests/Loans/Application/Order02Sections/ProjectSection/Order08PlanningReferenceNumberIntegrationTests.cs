using System.Diagnostics.CodeAnalysis;
using HE.Investments.Common.Messages;
using HE.Investments.IntegrationTestsFramework.Extensions;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.Project.Consts;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order08PlanningReferenceNumberIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order08PlanningReferenceNumberIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenReferenceNumberExceedsShortInputLimit()
    {
        // given
        var planningReferenceNumberPage = await TestClient.NavigateTo(ProjectPagesUrls.PlanningReferenceNumber(_applicationLoanId, _projectId));
        var continueButton = planningReferenceNumberPage.GetGdsSubmitButtonById("continue-button");

        // when
        planningReferenceNumberPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "PlanningReferenceNumber", TextTestData.TextThatExceedsShortInputLimit } });

        // then
        planningReferenceNumberPage
            .UrlEndWith(ProjectPagesUrls.PlanningReferenceNumberSuffix)
            .HasLabelTitle(ProjectPageTitles.PlanningReferenceNumber)
            .HasOneValidationMessages(ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.PlanningReferenceNumber));

        SetSharedData(SharedKeys.CurrentPageKey, planningReferenceNumberPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldRedirectToPlanningStatus_WhenReferenceNumberWasNotProvided()
    {
        // given
        var planningReferenceNumberPage = await GetCurrentPage(() => TestClient.NavigateTo(ProjectPagesUrls.PlanningReferenceNumber(_applicationLoanId, _projectId)));
        var continueButton = planningReferenceNumberPage.GetGdsSubmitButtonById("continue-button");

        // when
        var planningStatus = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "PlanningReferenceNumber", string.Empty } });

        // then
        planningStatus
            .UrlEndWith(ProjectPagesUrls.PlanningPermissionStatusSuffix)
            .HasTitle(ProjectPageTitles.PlanningPermissionStatus);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldRedirectToPlanningStatus_WhenReferenceNumberDoesNotExceedsShortInputLimit()
    {
        // given
        var planningReferenceNumberPage = await GetCurrentPage(() => TestClient.NavigateTo(ProjectPagesUrls.PlanningReferenceNumber(_applicationLoanId, _projectId)));
        var continueButton = planningReferenceNumberPage.GetGdsSubmitButtonById("continue-button");

        // when
        var planningStatus = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "PlanningReferenceNumber", TextTestData.TextThatNotExceedsShortInputLimit } });

        // then
        planningStatus
            .UrlEndWith(ProjectPagesUrls.PlanningPermissionStatusSuffix)
            .HasTitle(ProjectPageTitles.PlanningPermissionStatus);
    }
}
