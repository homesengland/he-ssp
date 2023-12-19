using System.Globalization;
using AngleSharp.Html.Dom;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Utils.Pagination;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.Common.WWWTestsFramework.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.Application;

public class ApplicationListTests : ViewTestBase
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
        var application1 = new ApplicationBasicDetails("1", "Application 1", ApplicationStatus.ApplicationSubmitted, "Local Authority 1", 10, 12);
        var application2 = new ApplicationBasicDetails("2", "Application 2", ApplicationStatus.Draft, null, 20, null);

        var applicationListModel = new ApplicationsListModel("Organisation Name", PaginationResult(new List<ApplicationBasicDetails> { application1, application2, }), false);

        // when
        var document = await Render(_viewPath, applicationListModel);

        // then
        ShouldDisplayPage(document);
        ShouldDisplayApplicationInTableRow(document, application1);
        ShouldDisplayApplicationInTableRow(document, application2);
    }

    private static void ShouldDisplayApplicationInTableRow(IHtmlDocument document, ApplicationBasicDetails application)
    {
        document.HasElementWithText("a", application.Name)
            .HasElementWithText("td", application.LocalAuthority ?? GenericMessages.NotProvided)
            .HasElementWithText("td", application.Grant.IsProvided() ? $"\u00a3{application.Grant.ToWholeNumberString()}" : "-")
            .HasElementWithText("td", application.Unit?.ToString(CultureInfo.InvariantCulture) ?? "-")
            .HasElementWithText("strong", application.Status.GetDescription());
    }

    private static void ShouldDisplayPage(IHtmlDocument document)
    {
        document
            .HasElementWithText("h1", "AHP 21-26 CME applications")
            .HasElementWithText("h2", "Your applications")
            .HasElementWithText("h2", "Affordable Homes Programme 2021-2026 CME")
            .HasElementWithText(
                "p",
                "You can start a new Affordable Homes Programme application here. This will not affect any of your previous applications.")
            .HasElementWithText("a", "Start new application");
    }

    private PaginationResult<ApplicationBasicDetails> PaginationResult(IList<ApplicationBasicDetails> items) => new(items, 1, 10, 100);
}
