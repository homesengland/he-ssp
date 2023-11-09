using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Tests.Helpers;
using HE.Investments.Common.WWW.Models.TaskList;
using HE.Investments.Common.WWW.TagHelpers;

namespace HE.Investment.AHP.WWW.Tests.Views.Application;

public class TaskListTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Application/TaskList.cshtml";

    [Fact]
    public async Task ShouldDisplayView_WhenSectionsAreMissing()
    {
        // given
        var model = new ApplicationModel("some site", "application xyz", new List<TaskListSectionModel>());

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document, model, false, "You have completed");
    }

    [Fact]
    public async Task ShouldDisplayView_WhenIncompleteSectionExist()
    {
        // given
        var model = new ApplicationModel(
            "some site",
            "application xyz",
            new List<TaskListSectionModel>
            {
                new("S1", "D1", "A1", "AC1", SectionStatus.NotStarted),
                new("S2", "D2", "A2", "AC2", SectionStatus.Completed),
                new("S3", "D3", "A3", "AC3", SectionStatus.InProgress),
            });

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document, model, true, "You have completed 1 of 3 sections");
    }

    [Fact]
    public async Task ShouldDisplayView_WhenNoIncompleteSection()
    {
        // given
        var model = new ApplicationModel(
            "some site",
            "application xyz",
            new List<TaskListSectionModel>
            {
                new("S1", "D1", "A1", "AC1", SectionStatus.Withdrawn),
                new("S2", "D2", "A2", "AC2", SectionStatus.Completed),
                new("S3", "D3", "A3", "AC3", SectionStatus.Submitted),
            });

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document, model, false, "You have completed");
    }

    private static void AssertView(IHtmlDocument document, ApplicationModel model, bool incompleteSectionsExist, string incompleteText)
    {
        document
            .HasElementWithText("span", model.SiteName)
            .HasElementWithText("h1", model.Name)
            .HasElementWithText("p", incompleteText, incompleteSectionsExist)
            .HasElementWithText("div", "You must complete all the sections before you can submit your application.", incompleteSectionsExist)
            .HasElementWithText("button", "Return to applications");
    }
}
