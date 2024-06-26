using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Views.Delivery.Const;
using HE.Investment.AHP.WWW.Views.FinancialDetails.Consts;
using HE.Investment.AHP.WWW.Views.HomeTypes.Const;
using HE.Investment.AHP.WWW.Views.Project.Const;
using HE.Investment.AHP.WWW.Views.Scheme.Const;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.Order04ChangeApplicationStatus;

[Order(407)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order07ReferredBackToApplicant : AhpIntegrationTest
{
    public Order07ReferredBackToApplicant(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldChangeApplicationStatusToReferredBackToApplicant()
    {
        // given && when
        await ChangeApplicationStatus(ApplicationData.ApplicationId, ApplicationStatus.ReferredBackToApplicant);
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId));

        // then
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasStatusTagByTestId(ApplicationStatus.ReferredBackToApplicant.GetDescription(), "application-status")
            .HasImportantNotificationBanner("You can now edit and resubmit your application.")
            .HasSectionWithStatus("enter-scheme-information-status", SectionStatus.InProgress.GetDescription())
            .HasSectionWithStatus("add-home-types-status", SectionStatus.InProgress.GetDescription())
            .HasSectionWithStatus("enter-financial-details-status", SectionStatus.InProgress.GetDescription())
            .HasSectionWithStatus("add-delivery-phases-status", SectionStatus.InProgress.GetDescription());
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldSubmitSchemeInformationSectionAgain()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(SchemeInformationPagesUrl.CheckAnswers(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId));
        checkAnswersPage
            .UrlEndWith(SchemeInformationPagesUrl.CheckAnswers(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId))
            .HasTitle(SchemeInformationPageTitles.CheckAnswers)
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var taskListPage = await TestClient.SubmitButton(
            continueButton,
            ("IsCompleted", true.MapToCommonResponse()));

        // then
        taskListPage.UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasSectionWithStatus("enter-scheme-information-status", SectionStatus.Completed.GetDescription());
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldSubmitHomeTypesSectionAgain()
    {
        // given
        var finishHomeTypesPage = await GetCurrentPage(HomeTypesPagesUrl.FinishHomeTypes(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId));
        finishHomeTypesPage
            .UrlEndWith(HomeTypesPagesUrl.FinishHomeTypes(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId))
            .HasTitle(HomeTypesPageTitles.FinishHomeTypes)
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var taskListPage = await TestClient.SubmitButton(continueButton, ("FinishAnswer", true.MapToCommonResponse()));

        // then
        taskListPage.UrlEndWith(ApplicationPagesUrl.TaskList(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId))
            .HasSectionWithStatus("add-home-types-status", SectionStatus.Completed.GetDescription());
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldSubmitFinancialDetailsSectionAgain()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(FinancialDetailsPagesUrl.CheckAnswers(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId));
        checkAnswersPage
            .UrlEndWith(FinancialDetailsPagesUrl.CheckAnswersSuffix)
            .HasTitle(FinancialDetailsPageTitles.CheckAnswers)
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var taskListPage = await TestClient.SubmitButton(
            continueButton,
            ("IsSectionCompleted", true.MapToCommonResponse()));

        // then
        taskListPage.UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasSectionWithStatus("enter-financial-details-status", SectionStatus.Completed.GetDescription());
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldSubmitDeliveryPhasesSectionAgain()
    {
        // given
        var completeDeliveryPhasesPage =
            await GetCurrentPage(DeliveryPhasesPagesUrl.CompleteDeliveryPhases(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId));
        completeDeliveryPhasesPage
            .UrlEndWith(DeliveryPhasesPagesUrl.CompleteDeliveryPhases(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId))
            .HasTitle(DeliveryPageTitles.Complete)
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var taskListPage = await TestClient.SubmitButton(continueButton, ("IsDeliveryCompleted", true.MapToCommonResponse()));

        // then
        taskListPage.UrlEndWith(ApplicationPagesUrl.TaskList(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId))
            .HasSectionWithStatus("add-delivery-phases-status", SectionStatus.Completed.GetDescription());
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldSubmitApplicationAgain_WhenAllSectionsAreCompleted()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId));
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasStatusTagByTestId(ApplicationStatus.ReferredBackToApplicant.GetDescription(), "application-status")
            .HasImportantNotificationBanner("You can now edit and resubmit your application.")
            .HasLinkWithTestId("check-and-submit-application", out var checkAndSubmitLink);

        // when
        var checkAnswersPage = await TestClient.NavigateTo(checkAndSubmitLink);
        var submitPage = await TestClient.SubmitButton(checkAnswersPage.GetSubmitButton("Accept and submit"));
        var completedPage = await TestClient.SubmitButton(
            submitPage.GetSubmitButton("Accept and submit"),
            (nameof(ApplicationSubmitModel.RepresentationsAndWarranties), "checked"));
        var applicationsPage = await TestClient.NavigateTo(completedPage.GetLinkByTestId("return-to-applications"));

        // then
        applicationsPage
            .UrlEndWith(ProjectPagesUrl.ProjectApplicationList(UserOrganisationData.OrganisationId, ShortGuid.FromString(ProjectData.ProjectId).Value))
            .HasTitle(ProjectPageTitles.ApplicationList(ProjectData.ProjectName));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ShouldHaveUnderReviewStatus_WhenApplicationWasSubmittedFromReferredBackToApplicantStatus()
    {
        // given && when
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId));

        // then
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasStatusTagByTestId(ApplicationStatus.UnderReview.GetDescription(), "application-status");
    }
}
