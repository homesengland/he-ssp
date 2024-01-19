using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class FloorAreaTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/FloorArea.cshtml";

    private static readonly FloorAreaModel Model = new("My application", "My homes");

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasPageHeader("My application - My homes", "Floor area")
            .HasElementWithText("h2", "Enter the internal floor area of this home type")
            .HasInput("FloorArea")
            .HasElementWithText("h2", "Do all of the homes of this home type meet all of the Nationally Described Space Standards?")
            .HasRadio(
                "MeetNationallyDescribedSpaceStandards",
                new[]
                {
                    "Yes",
                    "No",
                })
            .HasElementWithText("span", "What are the Nationally Described Space Standards?")
            .HasElementWithText("a", "Read more about the Nationally Described Space Standards (opens in a new tab).")
            .HasGdsSaveAndContinueButton();
    }
}
