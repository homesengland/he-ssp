using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;

namespace HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

public class ModernMethodsOfConstructionBuilder
{
    private readonly List<ModernMethodsConstructionCategoriesType> _modernMethodsConstructionCategories = new();
    private readonly List<ModernMethodsConstruction2DSubcategoriesType> _modernMethodsConstruction2DSubcategories = new();
    private readonly List<ModernMethodsConstruction3DSubcategoriesType> _modernMethodsConstruction3DSubcategories = new();

    public static ModernMethodsOfConstructionBuilder New() => new();

    public ModernMethodsOfConstructionBuilder With2DCategory()
    {
        _modernMethodsConstructionCategories.Add(ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems);

        return this;
    }

    public ModernMethodsOfConstructionBuilder With3DCategory()
    {
        _modernMethodsConstructionCategories.Add(ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems);

        return this;
    }

    public ModernMethodsOfConstruction BuildValid2DAnd3DCategories()
    {
        return new ModernMethodsOfConstruction(
            new[]
            {
                ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems,
                ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems,
            }.ToList(),
            new[] { ModernMethodsConstruction2DSubcategoriesType.EnhancedConsolidation }.ToList(),
            new[] { ModernMethodsConstruction3DSubcategoriesType.StructuralChassisAndInternallyFittedOut }.ToList());
    }

    public ModernMethodsOfConstruction BuildValidOtherCategories()
    {
        return new ModernMethodsOfConstruction(
            new[]
            {
                ModernMethodsConstructionCategoriesType.Category5PreManufacturingNonStructuralAssembliesAndSubAssemblies,
                ModernMethodsConstructionCategoriesType.Category6TraditionalBuildingProductLedSiteLabourReductionOrProductivityImprovements,
            }.ToList(),
            new List<ModernMethodsConstruction2DSubcategoriesType>(),
            new List<ModernMethodsConstruction3DSubcategoriesType>());
    }

    public ModernMethodsOfConstruction Build()
    {
        return new ModernMethodsOfConstruction(
            _modernMethodsConstructionCategories,
            _modernMethodsConstruction2DSubcategories,
            _modernMethodsConstruction3DSubcategories);
    }
}
