using AngleSharp.Html.Dom;
using HE.Investments.Account.WWW.Models.UserOrganisation;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Xunit;

namespace HE.Investments.Account.WWW.Tests.Views.UserOrganisation;

public class ApplicationsCardTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/UserOrganisation/_ApplicationsCard.cshtml";

    [Fact]
    public async Task ShouldDisplayApplicationsCard_ForMoreThan5Applications()
    {
        // given
        var model = CreateTestModel();

        // when
        var document = await Render(_viewPath, model);

        // then
        document
            .HasElementWithText("h3", $"Applications")
            .HasElementWithText("p", "2 other(s)")
            .HasElementWithText("a", "View all");

        AssertApplications(model, document);
    }

    [Fact]
    public async Task ShouldDisplayApplicationsCard_ForLessThan5Applications()
    {
        // given
        var model = CreateTestModel(new[]
        {
            new ApplicationBasicDetailsModel(Guid.NewGuid().ToString(), "Ap1", ApplicationStatus.New, "http://localhost/app/"),
            new ApplicationBasicDetailsModel(Guid.NewGuid().ToString(), "Ap2", ApplicationStatus.New, "http://localhost/app/"),
            new ApplicationBasicDetailsModel(Guid.NewGuid().ToString(), "Ap3", ApplicationStatus.ApplicationDeclined, "http://localhost/app/"),
        });

        // when
        var document = await Render(_viewPath, model);

        // then
        document
            .HasElementWithText("h3", $"Applications")
            .HasElementWithText("p", "other(s)", false)
            .HasElementWithText("a", "View all");

        AssertApplications(model, document);
    }

    [Fact]
#pragma warning disable S2699 // Tests should include assertions
    public async Task ShouldNotDisplayApplicationsCard_ForMissingApplications()
#pragma warning restore S2699 // Tests should include assertions
    {
        // given
        var model = CreateTestModel(new List<ApplicationBasicDetailsModel>());

        // when
        var document = await Render(_viewPath, model);

        // then
        document
            .HasElementWithText("h3", $"Applications", false)
            .HasElementWithText("p", "other(s)", false)
            .HasElementWithText("a", "View all", false);
    }

    private static void AssertApplications(UserApplicationsModel model, IHtmlDocument document)
    {
        for (var i = 0; i < model.Applications.Count; i++)
        {
            document.HasElementWithText("a", model.Applications[i].ApplicationName, i < 5);
        }
    }

    private static ApplicationBasicDetailsModel[] ApplicationBasicDetailsModels()
    {
        return new[]
        {
            new ApplicationBasicDetailsModel(Guid.NewGuid().ToString(), "Ap1", ApplicationStatus.New, "http://localhost/app/"),
            new ApplicationBasicDetailsModel(Guid.NewGuid().ToString(), "Ap2", ApplicationStatus.New, "http://localhost/app/"),
            new ApplicationBasicDetailsModel(Guid.NewGuid().ToString(), "Ap3", ApplicationStatus.ApplicationDeclined, "http://localhost/app/"),
            new ApplicationBasicDetailsModel(Guid.NewGuid().ToString(), "Ap4", ApplicationStatus.New, "http://localhost/app/"),
            new ApplicationBasicDetailsModel(Guid.NewGuid().ToString(), "Ap5", ApplicationStatus.Draft, "http://localhost/app/"),
            new ApplicationBasicDetailsModel(Guid.NewGuid().ToString(), "Ap6", ApplicationStatus.New, "http://localhost/app/"),
            new ApplicationBasicDetailsModel(Guid.NewGuid().ToString(), "Ap7", ApplicationStatus.New, "http://localhost/app/"),
        };
    }

    private UserApplicationsModel CreateTestModel(IList<ApplicationBasicDetailsModel>? applications = null)
    {
        return new UserApplicationsModel(
            "Nagłówek",
            applications ?? ApplicationBasicDetailsModels(),
            "A");
    }
}
