using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class FloorAreaTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/FloorArea.cshtml";

    private static readonly FloorAreaModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        IList<string> options = [
            "Yes",
            "No",
        ];

        document
            .HasPageHeader("My application - My homes", "Floor area")
            .HasElementWithText("h2", "Enter the internal floor area of this home type")
            .HasInput("FloorArea")
            .HasElementWithText("h2", "Do all of the homes of this home type meet all of the Nationally Described Space Standards?")
            .HasRadio("MeetNationallyDescribedSpaceStandards", options)
            .HasElementWithText("span", "What are the Nationally Described Space Standards?")
            .HasElementWithText("a", "Read more about the Nationally Described Space Standards (opens in a new tab).")
            .HasSaveAndContinueButton();
    }
}
