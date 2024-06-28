using System.Globalization;
using AngleSharp.Html.Dom;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Project;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investment.AHP.WWW.Tests.Views.Project;

public class ProjectApplicationListTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/Project/ListOfApplications.cshtml";

    [Fact]
    public async Task ShouldDisplayView_WhenThereIsNoApplications()
    {
        // given
        var applicationListModel = new ProjectDetailsModel(
            new FrontDoorProjectId("project-id"),
            "Carq's project",
            "Affordable Homes Programme Continuous Market Engagement 2021-2026",
            "Organisation Name",
            PaginationResult([]),
            false);

        // when
        var document = await Render(_viewPath, applicationListModel);

        // then
        ShouldDisplayPage(document);
    }

    [Fact]
    public async Task ShouldDisplayView_WhenThereAreTwoApplications()
    {
        // given
        var application1 = new ApplicationProjectModel(AhpApplicationId.From("1"), "Application 1", ApplicationStatus.ApplicationSubmitted, 1564553, 12);
        var application2 = new ApplicationProjectModel(AhpApplicationId.From("2"), "Application 2", ApplicationStatus.Draft, 266468, null);

        var applicationListModel = new ProjectDetailsModel(
            new FrontDoorProjectId("project-id"),
            "Carq's project",
            "Affordable Homes Programme Continuous Market Engagement 2021-2026",
            "Organisation Name",
            PaginationResult([application1, application2,]),
            false);

        // when
        var document = await Render(_viewPath, applicationListModel);

        // then
        ShouldDisplayPage(document);
        ShouldDisplayApplicationInTableRow(document, application1, "£1,564,553");
        ShouldDisplayApplicationInTableRow(document, application2, "£266,468");
    }

    private static void ShouldDisplayApplicationInTableRow(IHtmlDocument document, ApplicationProjectModel application, string expectedGrant)
    {
        document.HasElementWithText("a", application.Name)
            .HasElementWithText("td", expectedGrant)
            .HasElementWithText("td", application.Unit?.ToString(CultureInfo.InvariantCulture) ?? "-")
            .HasElementWithText("strong", application.Status.GetDescription());
    }

    private static void ShouldDisplayPage(IHtmlDocument document)
    {
        document
            .HasElementWithText("h1", "Carq's project")
            .HasElementWithText("h2", "Affordable Homes Programme Continuous Market Engagement 2021-2026 applications")
            .HasElementWithText("h2", "Apply for Affordable Homes Programme Continuous Market Engagement 2021-2026")
            .HasLinkButton("Start")
            .HasLinkButton("View");
    }

    private PaginationResult<ApplicationProjectModel> PaginationResult(IList<ApplicationProjectModel> items) => new(items, 1, 10, 100);
}
