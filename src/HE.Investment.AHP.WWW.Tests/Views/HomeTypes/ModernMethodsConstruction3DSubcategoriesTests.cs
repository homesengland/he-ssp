using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class ModernMethodsConstruction3DSubcategoriesTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/ModernMethodsConstruction3DSubcategories.cshtml";

    [Fact]
    public async Task ShouldRenderViewWithCheckboxes()
    {
        // given
        var model = new ModernMethodsConstructionModel("My application", "My homes");

        // when
        var document = await RenderHomeTypePage(ViewPath, model);

        // then
        IList<string> options = [
            "StructuralChassisOnly",
            "StructuralChassisAndInternallyFittedOut",
            "StructuralChassisInternallyFittedOutAndExternalCladdingOrRoofingCompleted",
            "StructuralChassisInternallyFittedOutAndPoddedRoomAssembled",
        ];

        document
            .HasPageHeader("My application - My homes", "Category 1")
            .HasElementWithText("h2", "Which of these sub-categories of 3D primary structural systems are you using?")
            .HasElementWithText("span", "Select all that apply.")
            .HasCheckboxes("ModernMethodsConstruction3DSubcategories", options)
            .HasSaveAndContinueButton();
    }

    [Fact]
    public async Task ShouldRenderViewWithCheckedCheckboxes()
    {
        // given
        var model = new ModernMethodsConstructionModel("My application", "My homes")
        {
            ModernMethodsConstruction3DSubcategories =
            [
                ModernMethodsConstruction3DSubcategoriesType.StructuralChassisAndInternallyFittedOut,
                ModernMethodsConstruction3DSubcategoriesType.StructuralChassisInternallyFittedOutAndPoddedRoomAssembled,
            ],
        };

        // when
        var document = await RenderHomeTypePage(ViewPath, model);

        // then
        IList<string> checkedValues = [
            "StructuralChassisAndInternallyFittedOut",
            "StructuralChassisInternallyFittedOutAndPoddedRoomAssembled",
        ];

        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Category 1")
            .HasElementWithText("h2", "Which of these sub-categories of 3D primary structural systems are you using?")
            .HasElementWithText("span", "Select all that apply.")
            .HasCheckedCheckboxes("ModernMethodsConstruction3DSubcategories", checkedValues)
            .HasSaveAndContinueButton();
    }
}
