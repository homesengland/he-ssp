using AngleSharp.Html.Dom;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.WWW.Models;
using HE.InvestmentLoans.WWW.Models.UserOrganisation;
using HE.InvestmentLoans.WWW.Tests.Helpers;
using Xunit;

namespace HE.InvestmentLoans.WWW.Tests.Views.UserOrganisation;

public class UserOrganisationTests : ViewTestBase
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
        var model = CreateTestModel(programmesToAccess: new List<ProgrammeToAccessModel>());

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertUserOrganisation(document, model, programmesToAccessExist: false);
    }

    [Fact]
    public async Task ShouldDisplayUserOrganisation_ForMissingProgrammesToApply()
    {
        // given
        var model = CreateTestModel(programmesToApply: new List<ProgrammeModel>());

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertUserOrganisation(document, model, programmesToApplyExist: false);
    }

    [Fact]
    public async Task ShouldDisplayUserOrganisation_ForMissingActions()
    {
        // given
        var model = CreateTestModel(actions: new List<ActionModel>());

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertUserOrganisation(document, model, actionsExist: false);
    }

    private static IHtmlDocument AssertUserOrganisation(
        IHtmlDocument document,
        UserOrganisationModel model,
        bool isLimitedUser = true,
        bool programmesToAccessExist = true,
        bool programmesToApplyExist = true,
        bool actionsExist = true)
    {
        document
            .HasElementWithText("p", $"Welcome {model.UserName}")
            .HasElementWithText("h3", $"{model.OrganisationName}'s Homes England account")
            .HasElementWithText("div", "Your request to be part of", isLimitedUser)
            .HasElementWithText("h3", $"Programmes you can access in {model.OrganisationName}", programmesToAccessExist)
            .HasElementWithText("p", "You have not yet applied for a service. To apply, select a funding programme below.", !programmesToAccessExist)
            .HasElementWithText("h3", "Programmes you can apply for", programmesToApplyExist);

        if (actionsExist)
        {
            document.HasElementWithText("a", model.Actions.First().Label);
        }

        return document;
    }

    private UserOrganisationModel CreateTestModel(
        string? orgName = null,
        string? userName = null,
        bool isLimitedUser = true,
        List<ProgrammeToAccessModel>? programmesToAccess = null,
        List<ProgrammeModel>? programmesToApply = null,
        List<ActionModel>? actions = null)
    {
        return new(
            orgName ?? "Organizacja Narodów Śląskich",
            userName ?? "Jan Muzykant",
            isLimitedUser,
            programmesToAccess ?? new List<ProgrammeToAccessModel>
            {
                new(
                    new ProgrammeModel("P1", "Desc1", "C", "V", "Ct"),
                    new List<ApplicationBasicDetailsModel> { new(Guid.NewGuid(), "AP1", ApplicationStatus.Withdrawn) }),
            },
            programmesToApply ?? new List<ProgrammeModel> { new ProgrammeModel("P2", "D2", "C", "V", "Ct") },
            actions ?? new List<ActionModel> { new ActionModel("Action Name", "A", "C") });
    }
}
