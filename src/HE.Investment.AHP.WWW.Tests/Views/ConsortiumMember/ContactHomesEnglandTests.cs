using HE.Investment.AHP.WWW.Models.Consortium;
using HE.Investment.AHP.WWW.Views.ConsortiumMember.Const;
using HE.Investments.AHP.Consortium.Contract.Queries;

namespace HE.Investment.AHP.WWW.Tests.Views.ConsortiumMember;

public class ContactHomesEnglandTests : AhpViewTestBase
{
    private static readonly AvailableProgramme Programme = new(
        Guid.NewGuid().ToString(),
        "Homes England Programme",
        "Very important Homes England Programme",
        "HE Programme");

    private readonly string _viewPath = "/Views/ConsortiumMember/ContactHomesEngland.cshtml";

    private readonly ConsortiumSelectedProgrammeModel _model = new(Guid.NewGuid().ToString(), Programme);

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var document = await Render(_viewPath, _model);

        // then
        document
            .HasTitle(ConsortiumMemberPageTitles.ContactHomesEngland(Programme.FullName, Programme.ShortName))
            .HasParagraph($"Based on your answers, your project is invited to apply for the {Programme.FullName} ({Programme.ShortName})")
            .HasElementWithText("a", "View the details of your consortium.")
            .HasParagraph("Only a lead partner of a consortium can apply. Speak to the lead partner of your consortium about including your proposal as part of your consortium's programme.")
            .HasParagraph("If you do not wish to apply for an affordable housing grant, please contact Homes England.")
            .HasDefaultButton("Return to your account");
    }
}
