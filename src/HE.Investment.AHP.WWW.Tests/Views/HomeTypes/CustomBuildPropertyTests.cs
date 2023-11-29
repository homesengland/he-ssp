using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class CustomBuildPropertyTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/CustomBuildProperty.cshtml";

    private static readonly CustomBuildPropertyBasicModel BasicModel = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, BasicModel);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Are the properties custom build?")
            .HasElementWithText("span", "What are custom build properties?")
            .HasElementWithText("a", "more information on custom build.")
            .HasRadio(
                "CustomBuild",
                new[]
                {
                    "Yes",
                    "No",
                })
            .HasElementWithText("button", "Save and continue");
    }
}
