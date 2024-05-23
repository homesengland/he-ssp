using AngleSharp.Html.Dom;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.WWW.Models.UserOrganisation;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Models;

namespace HE.Investments.Account.WWW.Tests.Views.UserOrganisation;

public class UserOrganisationTests : AccountViewTestBase
{
    private readonly string _viewPath = "/Views/UserOrganisation/UserOrganisation.cshtml";

    [Fact]
    public async Task ShouldDisplayUserOrganisation()
    {
        // given
        var model = CreateTestModel();

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertUserOrganisation(document, model);
    }

    [Fact]
    public async Task ShouldDisplayUserOrganisation_ForNotLimitedUser()
    {
        // given
        var model = CreateTestModel(isLimitedUser: false);

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertUserOrganisation(document, model, false);
    }

    [Fact]
    public async Task ShouldDisplayUserOrganisation_ForMissingProgrammesToAccess()
    {
        // given
        var model = CreateTestModel(programmesToAccess: []);

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertUserOrganisation(document, model, programmesToAccessExist: false);
    }

    [Fact]
    public async Task ShouldDisplayUserOrganisation_ForProgrammesToAccessWithMissingApplications()
    {
        // given
        var model = CreateTestModel(
            programmesToAccess:
            [
                new(
                    new ProgrammeModel(ProgrammeType.Ahp, "P1", "Desc1", "Title", "Label", "V"),
                    []),
            ]);

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertUserOrganisation(document, model, programmesToAccessExist: false);
    }

    [Fact]
    public async Task ShouldDisplayUserOrganisation_ForMissingActions()
    {
        // given
        var model = CreateTestModel(actions: []);

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertUserOrganisation(document, model, actionsExist: false);
    }

    private static void AssertUserOrganisation(
        IHtmlDocument document,
        UserOrganisationModel model,
        bool isLimitedUser = true,
        bool programmesToAccessExist = true,
        bool actionsExist = true)
    {
        document
            .HasParagraph($"Welcome {model.UserName}")
            .HasElementWithText("h1", $"{model.OrganisationName}'s Homes England account")
            .HasElementWithText("div", "Your request to join", isLimitedUser)
            .HasParagraph("You have not applied for any support. Select start a new project to apply for support.", !programmesToAccessExist)
            .HasElementWithText("h2", "Apply for support");

        if (actionsExist)
        {
            document.HasElementWithText("a", model.Actions[0].Label);
        }
    }

    private static List<ProgrammeToAccessModel> ProgrammesToToAccess()
    {
        return
        [
            new(
                new ProgrammeModel(ProgrammeType.Ahp, "P1", "Desc1", "Title", "Label", "V"),
                [new("1", "AP1", ApplicationStatus.Withdrawn, "http://localhost/app/")]),
        ];
    }

    private UserOrganisationModel CreateTestModel(
        string? orgName = null,
        string? userName = null,
        bool isLimitedUser = true,
        List<ProgrammeToAccessModel>? programmesToAccess = null,
        List<ActionModel>? actions = null)
    {
        return new(
            orgName ?? "Organizacja Narodów Śląskich",
            userName ?? "Jan Muzykant",
            isLimitedUser,
            "start-project",
            [],
            programmesToAccess ?? ProgrammesToToAccess(),
            actions ?? [new("ViewAllApplicationsUrl Name", "A", "C", true)]);
    }
}
