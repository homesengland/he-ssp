using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class AccessibilityCategoryTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/AccessibilityCategory.cshtml";

    private static readonly AccessibilityModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasPageHeader("My application - My homes", "Which accessibility categories do the homes meet?")
            .HasElementWithText("span", "What are the accessibility categories?")
            .HasElementWithText("a", "Read the Building Regulations to find out more (opens in the new tab).")
            .HasRadio(
                "AccessibilityCategory",
                new[]
                {
                    "M4(1) Category 1: Visitable dwellings",
                    "M4(2) Category 2: Accessible and adaptable dwellings",
                    "M4(3) Category 3: Wheelchair user dwellings",
                })
            .HasGdsSaveAndContinueButton();
    }
}
