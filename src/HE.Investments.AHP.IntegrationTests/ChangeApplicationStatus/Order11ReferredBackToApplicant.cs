using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Views.Application;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.ChangeApplicationStatus;

[Order(11)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order11ReferredBackToApplicant : AhpIntegrationTest
{
    public Order11ReferredBackToApplicant(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldSubmitApplicationAgain_WhenApplicationIsReferredBackToApplicant()
    {
        // given
        await ChangeApplicationStatus(ApplicationData.ApplicationId, ApplicationStatus.ReferredBackToApplicant);

        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId));
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
            .UrlEndWith(MainPagesUrl.ApplicationList)
            .HasTitle(ApplicationPageTitles.ApplicationList);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldHaveUnderReviewStatus_WhenApplicationWasSubmittedFromReferredBackToApplicantStatus()
    {
        // given && when
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId));

        // then
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasStatusTagByTestId(ApplicationStatus.UnderReview.GetDescription(), "application-status");
    }
}
