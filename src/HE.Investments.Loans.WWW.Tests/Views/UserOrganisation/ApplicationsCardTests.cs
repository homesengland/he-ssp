using AngleSharp.Html.Dom;
using HE.Investments.Common.Domain;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using HE.Investments.Loans.WWW.Models.UserOrganisation;
using Xunit;

namespace HE.Investments.Loans.WWW.Tests.Views.UserOrganisation;

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
            new ApplicationBasicDetailsModel(Guid.NewGuid(), "Ap1", ApplicationStatus.New),
            new ApplicationBasicDetailsModel(Guid.NewGuid(), "Ap2", ApplicationStatus.New),
            new ApplicationBasicDetailsModel(Guid.NewGuid(), "Ap3", ApplicationStatus.ApplicationDeclined),
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
    public async Task ShouldNotDisplayApplicationsCard_ForMissingApplications()
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
            new ApplicationBasicDetailsModel(Guid.NewGuid(), "Ap1", ApplicationStatus.New),
            new ApplicationBasicDetailsModel(Guid.NewGuid(), "Ap2", ApplicationStatus.New),
            new ApplicationBasicDetailsModel(Guid.NewGuid(), "Ap3", ApplicationStatus.ApplicationDeclined),
            new ApplicationBasicDetailsModel(Guid.NewGuid(), "Ap4", ApplicationStatus.New),
            new ApplicationBasicDetailsModel(Guid.NewGuid(), "Ap5", ApplicationStatus.Draft),
            new ApplicationBasicDetailsModel(Guid.NewGuid(), "Ap6", ApplicationStatus.New),
            new ApplicationBasicDetailsModel(Guid.NewGuid(), "Ap7", ApplicationStatus.New),
        };
    }

    private UserApplicationsModel CreateTestModel(IList<ApplicationBasicDetailsModel>? applications = null)
    {
        return new UserApplicationsModel(
            "Nagłówek",
            applications ?? ApplicationBasicDetailsModels(),
            "A",
            "C");
    }
}
