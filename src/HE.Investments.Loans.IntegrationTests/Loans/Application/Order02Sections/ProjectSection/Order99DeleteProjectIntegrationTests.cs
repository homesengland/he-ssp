using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.Loans.Contract.Projects.ViewModels;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW.Views.Project.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order99DeleteProjectIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;
    private readonly string? _projectId;

    public Order99DeleteProjectIntegrationTests(LoansIntegrationTestFixture fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = GetSharedDataOrNull<string>(SharedKeys.ProjectToDeleteId);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_RemoveLinkFromTaskListShouldRedirectToDeleteProjectPage()
    {
        // given
        var startProjectPage = await TestClient.NavigateTo(ProjectPagesUrls.Start(UserOrganisationData.OrganisationId, _applicationLoanId));

        var startButton = startProjectPage.GetGdsSubmitButtonById("start-now-button");
        var projectNamePage = await TestClient.SubmitButton(startButton);

        var projectId = projectNamePage.Url.GetProjectGuidFromUrl();
        var taskList = await TestClient.NavigateTo(ApplicationPagesUrls.TaskList(UserOrganisationData.OrganisationId, _applicationLoanId));
        var (name, _, removeUrl) = taskList.GetProjectFromTaskList(projectId);

        // when
        var deleteProjectPage = await TestClient.NavigateTo(removeUrl);

        // then
        deleteProjectPage
            .UrlEndWith(ProjectPagesUrls.DeleteSuffix)
            .HasTitle(ProjectPageTitles.Delete(name));

        SetCurrentPage(deleteProjectPage);

        SetSharedData(SharedKeys.ProjectToDeleteId, projectId);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldRedirectToTaskListAndDoNotDeleteProject_WhenNoAnswerWasSelected()
    {
        // given
        var deleteProjectPage = await GetCurrentPage(ProjectPagesUrls.Delete(UserOrganisationData.OrganisationId, _applicationLoanId, _projectId!));

        var continueButton = deleteProjectPage.GetGdsSubmitButtonById("continue-button");

        // when
        var taskList = await TestClient.SubmitButton(continueButton, (nameof(ProjectViewModel.DeleteProject), string.Empty));

        // then
        taskList
            .UrlEndWith(ApplicationPagesUrls.TaskListSuffix)
            .HasTitle(UserData.LoanApplicationName);

        taskList.ProjectExistsAtTaskList(_projectId!).Should().BeTrue();
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldRedirectToTaskListAndDoNotDeleteProject_WhenNoWasSelected()
    {
        // given
        var deleteProjectPage = await GetCurrentPage(ProjectPagesUrls.Delete(UserOrganisationData.OrganisationId, _applicationLoanId, _projectId!));

        var continueButton = deleteProjectPage.GetGdsSubmitButtonById("continue-button");

        // when
        var taskList = await TestClient.SubmitButton(continueButton, (nameof(ProjectViewModel.DeleteProject), CommonResponse.No));

        // then
        taskList
            .UrlEndWith(ApplicationPagesUrls.TaskListSuffix)
            .HasTitle(UserData.LoanApplicationName);

        taskList.ProjectExistsAtTaskList(_projectId!).Should().BeTrue();
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldRedirectToTaskListAndDoDeleteProjectAndShowNotification_WhenYesWasSelected()
    {
        // given
        var deleteProjectPage = await GetCurrentPage(ProjectPagesUrls.Delete(UserOrganisationData.OrganisationId, _applicationLoanId, _projectId!));

        var continueButton = deleteProjectPage.GetGdsSubmitButtonById("continue-button");

        // when
        var taskList = await TestClient.SubmitButton(continueButton, (nameof(ProjectViewModel.DeleteProject), CommonResponse.Yes));

        // then
        taskList
            .UrlEndWith(ApplicationPagesUrls.TaskListSuffix)
            .HasTitle(UserData.LoanApplicationName);

        taskList.ProjectExistsAtTaskList(_projectId!).Should().BeFalse();

        taskList.NotificationMessage().Should().Be($"New project removed");
    }
}
