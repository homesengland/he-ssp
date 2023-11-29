using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class BuildingInformationTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/BuildingInformation.cshtml";

    private static readonly BuildingInformationBasicModel BasicModel = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, BasicModel);

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
            .HasElementWithText("span", "A single self-contained residential dwelling, usually of one storey, withing a single structure containing multiple dwellings.")
            .HasElementWithText("span", "Single unit within a shared property.")
            .HasElementWithText("span", "A single self-contained residential dwelling that is only on one storey.")
            .HasElementWithText("span", "A single self-contained residential dwelling usually of two storeys with your own front door.")
            .HasElementWithText("button", "Save and continue");
    }
}
