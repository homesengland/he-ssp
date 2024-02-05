using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW;
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
    public async Task Order01_ShouldSubmitApplication_WhenApplicationIsCompletelyFilled()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId));
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasApplicationStatus(ApplicationStatus.Draft.GetDescription())
            .HasLinkWithTestId("check-and-submit-application", out var checkAndSubmitLink);

        // when
        var applicationCheckAnswersPage = await TestClient.NavigateTo(checkAndSubmitLink);

        // then
        var submitButton = applicationCheckAnswersPage
            .UrlEndWith(ApplicationPagesUrl.CheckAnswersSuffix)
            .HasTitle(ApplicationPageTitles.CheckAnswers)
            .GetSubmitButton("Submit");

        await TestClient.SubmitButton(submitButton);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldNavigateBackToTaskList_WhenApplicationWasSubmitted()
    {
        // given
        var applicationSubmittedPage = await GetCurrentPage();
        applicationSubmittedPage
            .UrlEndWith(ApplicationPagesUrl.SubmittedSuffix)
            .HasTitle(ApplicationPageTitles.Submitted)
            .HasLinkWithTestId("return-to-task-list", out var returnToTaskListLink);

        // when
        var taskListPage = await TestClient.NavigateTo(returnToTaskListLink);

        // then
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasApplicationStatus(ApplicationStatus.ApplicationSubmitted.GetDescription());

        SaveCurrentPage();
    }
}
