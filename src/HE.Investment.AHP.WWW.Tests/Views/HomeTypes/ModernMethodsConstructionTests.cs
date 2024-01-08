using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class ModernMethodsConstructionTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/ModernMethodsConstruction.cshtml";

    private static readonly ModernMethodsConstructionModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Are you using Modern Methods of Construction (MMC) for this home type?")
            .HasElementWithText("span", "Help with Modern Methods of Construction (MMC)")
            .HasElementWithText("a", "Read further information on the MMC categories (opens in a new tab).")
            .HasRadio(
                "ModernMethodsConstructionApplied",
                new[]
                {
                    "Yes",
                    "No",
                })
            .HasElementWithText("button", "Save and continue");
    }
}
