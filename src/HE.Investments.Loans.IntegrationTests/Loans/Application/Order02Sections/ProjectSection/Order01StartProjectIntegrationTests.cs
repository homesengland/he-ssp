using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.Project.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01StartProjectIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;

    public Order01StartProjectIntegrationTests(LoansIntegrationTestFixture fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
    }

    [SkippableFact(typeof(SkipException), Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldOpenProjectStartingPage_WhenCompanyStructureLinkIsClickedOnTaskListPage()
    {
        // given
        var taskList = await TestClient.NavigateTo(ApplicationPagesUrls.TaskList(_applicationLoanId));

        // when
        var linkToAddProject = taskList.GetAnchorElementById("add-project-link");
        var startProjectPage = await TestClient.NavigateTo(linkToAddProject);

        // then
        startProjectPage
            .UrlEndWith(ProjectPagesUrls.StartSuffix)
            .HasTitle(ProjectPageTitles.StartPage)
            .HasGdsSubmitButton("start-now-button", out _);

        SetSharedData(SharedKeys.CurrentPageKey, startProjectPage);
    }

    [SkippableFact(typeof(SkipException), Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldRedirectToProjectNameAndAddNewProjectToTaskList_WhenStartButtonIsClicked()
    {
        // given
        var startProjectPage = await GetCurrentPage(() => TestClient.NavigateTo(ProjectPagesUrls.Start(_applicationLoanId)));

        var startButton = startProjectPage.GetGdsSubmitButtonById("start-now-button");

        // when
        var projectNamePage = await TestClient.SubmitButton(startButton);

        // then
        projectNamePage
            .UrlEndWith(ProjectPagesUrls.NameSuffix)
            .HasTitle(ProjectPageTitles.Name);

        var projectId = projectNamePage.Url.GetProjectGuidFromUrl();

        var taskList = await TestClient.NavigateTo(ApplicationPagesUrls.TaskList(_applicationLoanId));

        var (name, status, _) = taskList.GetProjectFromTaskList(projectId);

        name.Should().Be("New project");
        status.Should().Be("Not Started");

        UserData.SetProjectId(projectId);
    }
}
