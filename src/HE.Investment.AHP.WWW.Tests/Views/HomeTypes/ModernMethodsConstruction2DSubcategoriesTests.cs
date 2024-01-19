using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class ModernMethodsConstruction2DSubcategoriesTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/ModernMethodsConstruction2DSubcategories.cshtml";

    [Fact]
    public async Task ShouldRenderViewWithCheckboxes()
    {
        // given
        var model = new ModernMethodsConstructionModel("My application", "My homes");

        // when
        var document = await RenderHomeTypePage(ViewPath, model);

        // then
        document
            .HasPageHeader("My application - My homes", "Category 2")
            .HasElementWithText("h2", "Which of these sub-categories of 2D primary structural systems are you using?")
            .HasElementWithText("span", "Select all that apply.")
            .HasCheckboxes(
                "ModernMethodsConstruction2DSubcategories",
                new[]
                {
                    "BasicFramingOnly",
                    "EnhancedConsolidation",
                    "FurtherEnhancedConsolidation",
                })
            .HasGdsSaveAndContinueButton();
    }

    [Fact]
    public async Task ShouldRenderViewWithCheckedCheckboxes()
    {
        // given
        var model = new ModernMethodsConstructionModel("My application", "My homes")
        {
            ModernMethodsConstruction2DSubcategories = new List<ModernMethodsConstruction2DSubcategoriesType>
            {
                ModernMethodsConstruction2DSubcategoriesType.BasicFramingOnly,
                ModernMethodsConstruction2DSubcategoriesType.EnhancedConsolidation,
            },
        };

        // when
        var document = await RenderHomeTypePage(ViewPath, model);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Category 2")
            .HasElementWithText("h2", "Which of these sub-categories of 2D primary structural systems are you using?")
            .HasElementWithText("span", "Select all that apply.")
            .HasCheckedCheckboxes(
                "ModernMethodsConstruction2DSubcategories",
                new[]
                {
                    "BasicFramingOnly",
                    "EnhancedConsolidation",
                })
            .HasGdsSaveAndContinueButton();
    }
}
