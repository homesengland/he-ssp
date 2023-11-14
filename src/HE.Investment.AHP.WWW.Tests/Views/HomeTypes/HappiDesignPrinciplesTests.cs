using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.WWW.Tests.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class HappiDesignPrinciplesTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/HappiDesignPrinciples.cshtml";

    private static readonly HappiDesignPrinciplesModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithCheckboxes()
    {
        // given & when
        var document = await Render(ViewPath, Model);

        // then
        document
            .HasElementWithText("div", "My application - My homes")
            .HasElementWithText("h1", "Which Housing our Ageing Population Panel for Innovation (HAPPI) design principles do the homes meet?")
            .HasElementWithText("span", "What are the HAPPI design principles?")
            .HasElementWithText("a", "View the HAPPI principles for more information.")
            .HasCheckboxes(
                "DesignPrinciples",
                new[]
                {
                    "AdaptabilityAndCareReadyDesign",
                    "BalconiesAndOutdoorSpace",
                    "DaylightInTheHomeAndInSharedSpaces",
                    "EnergyEfficiencyAndSustainableDesign",
                    "ExternalSharedSurfacedAndHomeZones",
                    "PlantsTreesAndTheNaturalEnvironment",
                    "PositiveUseOfCirculationSpace",
                    "SharedFacilitiesAndHubs",
                    "SpaceAndFlexibility",
                    "StorageForBelongingsAndBicycles",
                })
            .HasCheckboxes("OtherPrinciples", new[] { "NoneOfThese", })
            .HasElementWithText("button", "Save and continue");
    }
}
