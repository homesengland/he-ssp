using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW.Views.LoanApplicationV2.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order04SupportingDocuments;

[Order(4)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class SubmitSupportingDocumentsIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;

    public SubmitSupportingDocumentsIntegrationTests(LoansIntegrationTestFixture fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldRedirectToApplicationDashboardSupportingDocumentsAndChangeStatusToReferredBackToApplicant()
    {
        // given && when
        await LoanApplicationCrmRepository.ChangeApplicationStatus(_applicationLoanId, ApplicationStatus.ReferredBackToApplicant);
        var supportingDocumentsPage = await TestClient.NavigateTo(ApplicationPagesUrls.ApplicationDashboardSupportingDocuments(_applicationLoanId));

        // then
        supportingDocumentsPage
            .UrlEndWith(ApplicationPagesUrls.ApplicationDashboardSupportingDocuments(_applicationLoanId))
            .HasHeader2(LoanApplicationPageTitles.SupportingDocuments)
            .HasStatusTagByTestId(ApplicationStatus.ReferredBackToApplicant.GetDescription(), "application-status");

        SetSharedData(SharedKeys.CurrentPageKey, supportingDocumentsPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ProvideSupportingDocuments()
    {
        // given
        var supportingDocumentsPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var submitButton = supportingDocumentsPage.GetGdsSubmitButtonByTestId("submit-button");
        var supportingDocuments = UserData.GenerateSupportingDocuments();

        // when
        var nextPage = await TestClient.SubmitButton(
            submitButton,
            new[]
            {
                ("SupportingDocumentsFile", supportingDocuments[0]),
                ("SupportingDocumentsFile", supportingDocuments[1]),
            });

        // then
        nextPage
            .UrlEndWith(ApplicationPagesUrls.ApplicationDashboardSupportingDocuments(_applicationLoanId))
            .HasHeader2(LoanApplicationPageTitles.SupportingDocuments)
            .HasSuccessNotificationBanner("Files successfully uploaded")
            .HasUploadedFiles(supportingDocuments.Count)
            .HasStatusTagByTestId(ApplicationStatus.UnderReview.GetDescription(), "application-status");
    }
}
