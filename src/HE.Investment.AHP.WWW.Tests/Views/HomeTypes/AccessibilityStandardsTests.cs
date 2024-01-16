using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class AccessibilityStandardsTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/AccessibilityStandards.cshtml";

    private static readonly AccessibilityModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasPageHeader("My application - My homes", "Do these homes meet any of the Building Regulations Part M accessibility categories?")
            .HasElementWithText("span", "What are the accessibility categories?")
            .HasElementWithText("a", "Read the Building Regulations to find out more (opens in the new tab).")
            .HasRadio(
                "AccessibilityStandards",
                new[]
                {
                    "Yes",
                    "No",
                })
            .HasGdsSaveAndContinueButton();
    }
}
