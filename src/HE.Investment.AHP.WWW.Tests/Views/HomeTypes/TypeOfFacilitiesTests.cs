using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class TypeOfFacilitiesTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/TypeOfFacilities.cshtml";

    private static readonly TypeBasicOfFacilitiesModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasPageHeader("My application - My homes", "What type of facilities do the homes have?")
            .HasRadio(
                "FacilityType",
                new[]
                {
                    "Self-contained facilities",
                    "Shared facilities",
                    "Mix of self-contained snd shared facilities",
                })
            .HasHint("Resident has use of their own facilities, such as bathroom and kitchen, within their own home.")
            .HasHint("Residents have their own room or rooms but facilities, such as bathroom and kitchen, are shared with others.")
            .HasGdsSaveAndContinueButton();
    }
}
