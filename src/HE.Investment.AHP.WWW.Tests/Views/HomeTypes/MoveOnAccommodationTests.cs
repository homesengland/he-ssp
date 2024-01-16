using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class MoveOnAccommodationTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/MoveOnAccommodation.cshtml";

    private static readonly MoveOnAccommodationModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithRadioButton()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasPageHeader("My application - My homes", "Are these homes intended as move on accommodation?")
            .HasElementWithText("div", "Move on accommodation is temporary accommodation to help people living in hostels make the transition to independent living.")
            .HasRadio("IntendedAsMoveOnAccommodation", new[] { "Yes", "No", })
            .HasGdsSaveAndContinueButton();
    }
}
