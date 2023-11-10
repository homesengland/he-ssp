using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.InvestmentLoans.WWW.Views.Project.Consts;
using HE.Investments.Common.Messages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.Application.Order02Sections.ProjectSection;

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
