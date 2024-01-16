using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class BuildingInformationTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/BuildingInformation.cshtml";

    private static readonly BuildingInformationModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasPageHeader("My application - My homes", "Building information")
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
            .HasGdsSaveAndContinueButton();
    }
}
