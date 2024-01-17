using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class ModernMethodsConstruction3DSubcategoriesTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/ModernMethodsConstruction3DSubcategories.cshtml";

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task ShouldRenderViewWithCheckboxes()
    {
        // given
        var model = new ModernMethodsConstructionModel("My application", "My homes");

        // when
        var document = await RenderHomeTypePage(ViewPath, model);

        // then
        document
            .HasPageHeader("My application - My homes", "Category 1")
            .HasElementWithText("h2", "Which of these sub-categories of 3D primary structural systems are you using?")
            .HasElementWithText("span", "Select all that apply.")
            .HasCheckboxes(
                "ModernMethodsConstruction3DSubcategories",
                new[]
                {
                    "StructuralChassisOnly",
                    "StructuralChassisAndInternallyFittedOut",
                    "StructuralChassisInternallyFittedOutAndExternalCladdingOrRoofingCompleted",
                    "StructuralChassisInternallyFittedOutAndPoddedRoomAssembled",
                })
            .HasGdsSaveAndContinueButton();
    }

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task ShouldRenderViewWithCheckedCheckboxes()
    {
        // given
        var model = new ModernMethodsConstructionModel("My application", "My homes")
        {
            ModernMethodsConstruction3DSubcategories = new List<ModernMethodsConstruction3DSubcategoriesType>
            {
                ModernMethodsConstruction3DSubcategoriesType.StructuralChassisAndInternallyFittedOut,
                ModernMethodsConstruction3DSubcategoriesType.StructuralChassisInternallyFittedOutAndPoddedRoomAssembled,
            },
        };

        // when
        var document = await RenderHomeTypePage(ViewPath, model);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Category 1")
            .HasElementWithText("h2", "Which of these sub-categories of 3D primary structural systems are you using?")
            .HasElementWithText("span", "Select all that apply.")
            .HasCheckedCheckboxes(
                "ModernMethodsConstruction3DSubcategories",
                new[]
                {
                    "StructuralChassisAndInternallyFittedOut",
                    "StructuralChassisInternallyFittedOutAndPoddedRoomAssembled",
                })
            .HasGdsSaveAndContinueButton();
    }
}
