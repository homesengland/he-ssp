using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class MoveOnAccommodationTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/MoveOnAccommodation.cshtml";

    private static readonly MoveOnAccommodationModel Model = new("My application", "My homes");

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task ShouldRenderViewWithRadioButton()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Are these homes intended as move on accommodation?")
            .HasElementWithText("div", "Move on accommodation is temporary accommodation to help people living in hostels make the transition to independent living.")
            .HasRadio("IntendedAsMoveOnAccommodation", new[] { "Yes", "No", })
            .HasElementWithText("button", "Save and continue");
    }
}
