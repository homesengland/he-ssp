using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using FluentAssertions;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using HE.Investments.IntegrationTestsFramework.Extensions;
using HE.Investments.Loans.Common.Extensions;
using HE.Investments.Loans.Contract.Projects.ViewModels;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order03SubmitApplication;

[Order(3)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class SubmitApplicationIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;

    public SubmitApplicationIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldOpenCheckAllAnswersPage_WhenAllApplicationSectionAreFilled()
    {
        // given
        var taskList = await TestClient.NavigateTo(ApplicationPagesUrls.TaskList(_applicationLoanId));

        await RemoveNonCompletedProjectsFrom(taskList);

        var submitLoanApplicationButton = taskList.GetGdsSubmitButtonById("submit-application");

        // when
        var checkApplicationPage = await TestClient.SubmitButton(submitLoanApplicationButton);

        // then
        checkApplicationPage
            .UrlEndWith(ApplicationPagesUrls.CheckApplicationSuffix)
            .HasTitle("Check your answers before submitting your application")
            .HasGdsSubmitButton("accept-and-submit", out _);

        SetSharedData(SharedKeys.CurrentPageKey, checkApplicationPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldMoveToApplicationSubmitted_WhenAcceptAndSubmitButtonIsClicked()
    {
        // given
        var checkApplicationPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var acceptAndSubmitButton = checkApplicationPage.GetGdsSubmitButtonById("accept-and-submit");

        // when
        var applicationSubmittedPage = await TestClient.SubmitButton(acceptAndSubmitButton);

        // then
        applicationSubmittedPage
            .UrlEndWith(ApplicationPagesUrls.ApplicationSubmittedSuffix)
            .HasGdsButton("application-submitted-to-dashboard");

        UserData.SetSubmittedLoanApplicationId(_applicationLoanId);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldDisplaySubmittedOnDate_WhenApplicationWasSubmitted()
    {
        // given && when
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrls.TaskList(_applicationLoanId));

        // then
        taskListPage
            .UrlEndWith(ApplicationPagesUrls.TaskListSuffix)
            .HasTitle(UserData.LoanApplicationName)
            .ExtractSubmittedOnDateFromTaskListPage(out var dateTime);

        dateTime.Should().BeCloseTo(DateTime.UtcNow.ConvertUtcToUkLocalTime(), TimeSpan.FromMinutes(2));
    }

    private async Task RemoveNonCompletedProjectsFrom(IHtmlDocument taskList)
    {
        var projects = taskList.GetTaskListProjects();

        var nonCompletedProjectRemoveUrls = projects.Where(p => p.Value.Status != "Completed").Select(p => p.Value.RemoveUrl);

        foreach (var removeUrl in nonCompletedProjectRemoveUrls)
        {
            var deleteProjectPage = await TestClient.NavigateTo(removeUrl);

            var continueButton = deleteProjectPage.GetGdsSubmitButtonById("continue-button");

            await TestClient.SubmitButton(continueButton, (nameof(ProjectViewModel.DeleteProject), CommonResponse.Yes));
        }
    }
}
