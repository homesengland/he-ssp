using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class CustomBuildPropertyTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/CustomBuildProperty.cshtml";

    private static readonly CustomBuildPropertyModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasPageHeader("My application - My homes", "Are the properties custom build?")
            .HasElementWithText("span", "What are custom build properties?")
            .HasElementWithText("a", "more information on custom build.")
            .HasRadio(
                "CustomBuild",
                new[]
                {
                    "Yes",
                    "No",
                })
            .HasSaveAndContinueButton();
    }
}
