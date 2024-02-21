using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Views.Application;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.ChangeApplicationStatus;

[Order(6)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order06Submit : AhpIntegrationTest
{
    public Order06Submit(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldNavigateToSubmitApplication_WhenApplicationIsCompletelyFilled()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId));
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasStatusTagByTestId(ApplicationStatus.Draft.GetDescription(), "application-status")
            .HasLinkWithTestId("check-and-submit-application", out var checkAndSubmitLink);

        // when
        var applicationCheckAnswersPage = await TestClient.NavigateTo(checkAndSubmitLink);

        // then
        var submitButton = applicationCheckAnswersPage
            .UrlEndWith(ApplicationPagesUrl.CheckAnswersSuffix)
            .HasTitle(ApplicationPageTitles.CheckAnswers)
            .GetSubmitButton("Accept and submit");

        await TestClient.SubmitButton(submitButton);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldNavigateToCompleted_WhenApplicationWasSubmitted()
    {
        await TestQuestionPage(
            ApplicationPagesUrl.Submit(ApplicationData.ApplicationId),
            ApplicationPagesUrl.SubmitSuffix,
            ApplicationPagesUrl.Completed(ApplicationData.ApplicationId),
            (nameof(ApplicationSubmitModel.RepresentationsAndWarranties), "checked"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldNavigateBackToTaskList_WhenApplicationWasSubmitted()
    {
        // given
        var applicationCompletedPage = await GetCurrentPage();
        applicationCompletedPage
            .UrlEndWith(ApplicationPagesUrl.CompletedSuffix)
            .HasTitle(ApplicationPageTitles.Completed)
            .HasLinkButton("Return to applications");

        // given & when
        var mainPage = await TestClient.NavigateTo(applicationCompletedPage.GetLinkButton("Return to applications"));

        // then
        mainPage
            .UrlEndWith(MainPagesUrl.ApplicationList)
            .HasTitle(ApplicationPageTitles.ApplicationList);
    }
}
