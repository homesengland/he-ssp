using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class ModernMethodsConstructionCategoriesTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/ModernMethodsConstructionCategories.cshtml";

    [Fact]
    public async Task ShouldRenderViewWithCheckboxes()
    {
        // given
        var model = new ModernMethodsConstructionModel("My application", "My homes");

        // when
        var document = await RenderHomeTypePage(ViewPath, model);

        // then
        document
            .HasPageHeader("My application - My homes", "Which Modern Methods of Construction (MMC) categories are you using?")
            .HasElementWithText("span", "Select all that apply.")
            .HasCheckboxes(
                "ModernMethodsConstructionCategories",
                [
                    "Category1PreManufacturing3DPrimaryStructuralSystems",
                    "Category2PreManufacturing2DPrimaryStructuralSystems",
                    "Category3PreManufacturedComponentNonSystemizedPrimaryStructure",
                    "Category4AdditiveManufacturingStructuringAndNonStructural",
                    "Category5PreManufacturingNonStructuralAssembliesAndSubAssemblies",
                    "Category6TraditionalBuildingProductLedSiteLabourReductionOrProductivityImprovements",
                    "Category7SiteProcessLedLabourReductionOrProductivityOrAssuranceImprovements",
                ])
            .HasSaveAndContinueButton();
    }

    [Fact]
    public async Task ShouldRenderViewWithCheckedCheckboxes()
    {
        // given
        var model = new ModernMethodsConstructionModel("My application", "My homes")
        {
            ModernMethodsConstructionCategories =
            [
                ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems,
                ModernMethodsConstructionCategoriesType.Category4AdditiveManufacturingStructuringAndNonStructural,
                ModernMethodsConstructionCategoriesType.Category5PreManufacturingNonStructuralAssembliesAndSubAssemblies,
                ModernMethodsConstructionCategoriesType.Category6TraditionalBuildingProductLedSiteLabourReductionOrProductivityImprovements,
            ],
        };

        // when
        var document = await RenderHomeTypePage(ViewPath, model);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Which Modern Methods of Construction (MMC) categories are you using?")
            .HasElementWithText("span", "Select all that apply.")
            .HasCheckedCheckboxes(
                "ModernMethodsConstructionCategories",
                [
                    "Category2PreManufacturing2DPrimaryStructuralSystems",
                    "Category4AdditiveManufacturingStructuringAndNonStructural",
                    "Category5PreManufacturingNonStructuralAssembliesAndSubAssemblies",
                    "Category6TraditionalBuildingProductLedSiteLabourReductionOrProductivityImprovements",
                ])
            .HasSaveAndContinueButton();
    }
}
