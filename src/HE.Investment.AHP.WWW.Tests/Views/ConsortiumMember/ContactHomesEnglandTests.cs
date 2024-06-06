using HE.Investment.AHP.WWW.Models.Consortium;
using HE.Investment.AHP.WWW.Views.ConsortiumMember.Const;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Enums;

namespace HE.Investment.AHP.WWW.Tests.Views.ConsortiumMember;

public class ContactHomesEnglandTests : AhpViewTestBase
{
    private static readonly Programme Programme = new(
        ProgrammeId.From(Guid.NewGuid().ToString()),
        "HEP",
        "Homes England Programme",
        true,
        ProgrammeType.Ahp,
        new DateRange(DateOnly.MinValue, DateOnly.MaxValue),
        new DateRange(DateOnly.MinValue, DateOnly.MaxValue),
        new DateRange(DateOnly.MinValue, DateOnly.MaxValue),
        new DateRange(DateOnly.MinValue, DateOnly.MaxValue));

    private readonly string _viewPath = "/Views/ConsortiumMember/ContactHomesEngland.cshtml";

    private readonly ConsortiumSelectedProgrammeModel _model = new(Guid.NewGuid().ToString(), Programme);

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var document = await Render(_viewPath, _model);

        // then
        document
            .HasTitle(ConsortiumMemberPageTitles.ContactHomesEngland(Programme.Name, Programme.ShortName))
            .HasParagraph($"Based on your answers, your project is invited to apply for the {Programme.Name} ({Programme.ShortName})")
            .HasElementWithText("a", "View the details of your consortium.")
            .HasParagraph("Only a lead partner of a consortium can apply. Speak to the lead partner of your consortium about including your proposal as part of your consortium's programme.")
            .HasParagraph("If you do not wish to apply for an affordable housing grant, please contact Homes England.")
            .HasLinkButton("Return to your account");
    }
}
