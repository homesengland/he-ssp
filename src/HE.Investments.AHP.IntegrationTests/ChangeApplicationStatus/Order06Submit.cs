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

[Order(6)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order06Submit : AhpIntegrationTest
{
    public Order06Submit(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
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
        // given
        var currentPage = await GetCurrentPage(ApplicationPagesUrl.Submit(ApplicationData.ApplicationId));
        currentPage
            .UrlWithoutQueryEndsWith(ApplicationPagesUrl.SubmitSuffix)
            .HasTitle(ApplicationPageTitles.Submit)
            .HasBackLink(out _)
            .HasSubmitButton(out var submitButton, "Accept and submit");

        // when
        var completedPage = await TestClient.SubmitButton(
            submitButton,
            (nameof(ApplicationSubmitModel.RepresentationsAndWarranties), "checked"));

        // then
        completedPage.UrlWithoutQueryEndsWith(ApplicationPagesUrl.CompletedSuffix)
            .HasTitle(ApplicationPageTitles.CompletedSecondTitle);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldNavigateBackToTaskList_WhenApplicationWasSubmitted()
    {
        // given
        var applicationCompletedPage = await GetCurrentPage();
        applicationCompletedPage
            .UrlEndWith(ApplicationPagesUrl.CompletedSuffix)
            .HasTitle(ApplicationPageTitles.CompletedSecondTitle)
            .HasLinkButton("Return to applications");

        // when
        var mainPage = await TestClient.NavigateTo(applicationCompletedPage.GetLinkButton("Return to applications"));

        // then
        mainPage
            .UrlEndWith(MainPagesUrl.ApplicationList)
            .HasTitle(ApplicationPageTitles.ApplicationList);
    }
}
