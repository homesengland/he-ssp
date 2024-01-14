using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class BuildingInformationTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/BuildingInformation.cshtml";

    private static readonly BuildingInformationModel Model = new("My application", "My homes");

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Building information")
            .HasRadio(
                "BuildingType",
                new[]
                {
                    "House",
                    "Flat",
                    "Bedsit",
                    "Bungalow",
                    "Maisonette",
                })
            .HasElementWithText("span", "A single self-contained residential dwelling, usually of more than one storey.")
            .HasElementWithText("span", "A single self-contained residential dwelling, usually of one storey, within a single structure containing multiple dwellings.")
            .HasElementWithText("span", "Single unit within a shared property.")
            .HasElementWithText("span", "A single self-contained residential dwelling that is only on one storey.")
            .HasElementWithText("span", "A single self-contained residential dwelling usually of two storeys with your own front door.")
            .HasElementWithText("button", "Save and continue");
    }
}
