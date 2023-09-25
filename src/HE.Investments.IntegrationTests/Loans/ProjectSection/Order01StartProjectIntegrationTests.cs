using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.InvestmentLoans.WWW.Views.Project.Consts;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01StartProjectIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;

    public Order01StartProjectIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = GetSharedData<string>(SharedKeys.ApplicationLoanIdInDraftStatusKey);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
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

    [Fact(Skip = "LoansConfig.SkipTest")]
    [Order(2)]
    public async Task Order02_ShouldRedirectToProjectNameAndAddNewProjectToTaskList_WhenStartButtonIsClicked()
    {
        // given
        //var startProjectPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);

        //var startButton = startProjectPage.GetGdsSubmitButtonById("start-now-button");

        //// when
        //var projectNamePage = await TestClient.SubmitButton(startButton);

        //// then
        //projectNamePage
        //    .UrlEndWith(ProjectPagesUrls.NameSuffix)
        //    .HasTitle(ProjectPageTitles.Name);

        //var projectId = projectNamePage.Url.GetProjectGuidFromUrl();

        var taskList = await TestClient.NavigateTo(ApplicationPagesUrls.TaskList(_applicationLoanId));

        taskList.GetTaskListProjects();
        // task

        //SetSharedData(SharedKeys.ProjectIdInDraftStatusKey, projectId);
    }
}
