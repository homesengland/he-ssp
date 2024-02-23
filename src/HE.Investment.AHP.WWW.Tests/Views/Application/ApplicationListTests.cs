using System.Globalization;
using AngleSharp.Html.Dom;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.WWW.Tests.Views.Application;

public class ApplicationListTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/Application/Index.cshtml";

    [Fact]
    public async Task ShouldDisplayView_WhenThereIsNoApplications()
    {
        // given
        var applicationListModel = new ApplicationsListModel("Organisation Name", PaginationResult(new List<ApplicationBasicDetails>()), false);

        // when
        var document = await Render(_viewPath, applicationListModel);

        // then
        ShouldDisplayPage(document);
    }

    [Fact]
    public async Task ShouldDisplayView_WhenThereAreTwoApplications()
    {
        // given
        var application1 = new ApplicationBasicDetails(AhpApplicationId.From("1"), "Application 1", ApplicationStatus.ApplicationSubmitted, "Local Authority 1", 1564553, 12);
        var application2 = new ApplicationBasicDetails(AhpApplicationId.From("2"), "Application 2", ApplicationStatus.Draft, null, 266468, null);

        var applicationListModel = new ApplicationsListModel("Organisation Name", PaginationResult(new List<ApplicationBasicDetails> { application1, application2, }), false);

        // when
        var document = await Render(_viewPath, applicationListModel);

        // then
        ShouldDisplayPage(document);
        ShouldDisplayApplicationInTableRow(document, application1, "£1,564,553");
        ShouldDisplayApplicationInTableRow(document, application2, "£266,468");
    }

    private static void ShouldDisplayApplicationInTableRow(IHtmlDocument document, ApplicationBasicDetails application, string expectedGrant)
    {
        document.HasElementWithText("a", application.Name)
            .HasElementWithText("td", application.LocalAuthority ?? GenericMessages.NotProvided)
            .HasElementWithText("td", expectedGrant)
            .HasElementWithText("td", application.Unit?.ToString(CultureInfo.InvariantCulture) ?? "-")
            .HasElementWithText("strong", application.Status.GetDescription());
    }

    private static void ShouldDisplayPage(IHtmlDocument document)
    {
        document
            .HasElementWithText("h1", "Affordable Homes Programme Continuous Market Engagement 2021-2026 applications")
            .HasElementWithText("h2", "Your applications")
            .HasElementWithText("h2", "Affordable Homes Programme Continuous Market Engagement 2021-2026")
            .HasElementWithText(
                "p",
                "You can start a new Affordable Homes Programme application. This will not affect any of your previous applications.")
            .HasLinkButton("Start");
    }

    private PaginationResult<ApplicationBasicDetails> PaginationResult(IList<ApplicationBasicDetails> items) => new(items, 1, 10, 100);
}
