using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class TypeOfFacilitiesTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/TypeOfFacilities.cshtml";

    private static readonly TypeBasicOfFacilitiesModel Model = new("My application", "My homes");

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "What type of facilities do the homes have?")
            .HasRadio(
                "FacilityType",
                new[]
                {
                    "Self-contained facilities",
                    "Shared facilities",
                    "Mix of self-contained snd shared facilities",
                })
            .HasElementWithText("span", "Resident has use of their own facilities, such as bathroom and kitchen, within their own home.")
            .HasElementWithText("span", "Residents have their own room or rooms but facilities, such as bathroom and kitchen, are shared with others.")
            .HasElementWithText("button", "Save and continue");
    }
}
