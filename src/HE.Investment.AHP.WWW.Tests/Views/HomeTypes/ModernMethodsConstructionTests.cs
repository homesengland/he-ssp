using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class ModernMethodsConstructionTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/ModernMethodsConstruction.cshtml";

    private static readonly ModernMethodsConstructionModel Model = new("My application", "My homes");

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasPageHeader("My application - My homes", "Are you using Modern Methods of Construction (MMC) for this home type?")
            .HasElementWithText("span", "Help with Modern Methods of Construction (MMC)")
            .HasElementWithText("a", "Read further information on the MMC categories (opens in a new tab).")
            .HasRadio(
                "ModernMethodsConstructionApplied",
                new[]
                {
                    "Yes",
                    "No",
                })
            .HasGdsSaveAndContinueButton();
    }
}
