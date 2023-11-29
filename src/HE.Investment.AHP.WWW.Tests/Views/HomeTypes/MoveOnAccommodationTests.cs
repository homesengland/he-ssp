using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class MoveOnAccommodationTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/MoveOnAccommodation.cshtml";

    private static readonly MoveOnAccommodationBasicModel BasicModel = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithRadioButton()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, BasicModel);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Are these homes intended as move on accommodation?")
            .HasElementWithText("div", "Move on accommodation is temporary accommodation to help people living in hostels make the transition to independent living.")
            .HasRadio("IntendedAsMoveOnAccommodation", new[] { "Yes", "No", })
            .HasElementWithText("button", "Save and continue");
    }
}
