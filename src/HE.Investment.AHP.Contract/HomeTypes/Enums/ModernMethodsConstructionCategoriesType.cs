using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes.Enums;

public enum ModernMethodsConstructionCategoriesType
{
    Undefined = 0,

    [Description("Category 1: Pre-manufacturing: 3D primary structural systems")]
    Category1PreManufacturing3DPrimaryStructuralSystems,

    [Description("Category 2: Pre-manufacturing: 2D primary structural systems")]
    Category2PreManufacturing2DPrimaryStructuralSystems,

    [Description("Category 3: Pre-manufactured component: Non-systemised primary structure")]
    Category3PreManufacturedComponentNonSystemizedPrimaryStructure,

    [Description("Category 4: Additive manufacturing: structuring and non-structural")]
    Category4AdditiveManufacturingStructuringAndNonStructural,

    [Description("Category 5: Pre-manufacturing: Non-structural assemblies and sub-assemblies")]
    Category5PreManufacturingNonStructuralAssembliesAndSubAssemblies,

    [Description("Category 6: Traditional building product led site labour reduction/productivity improvements")]
    Category6TraditionalBuildingProductLedSiteLabourReductionOrProductivityImprovements,

    [Description("Category 7: Site process led labour reduction/productivity/assurance improvements")]
    Category7SiteProcessLedLabourReductionOrProductivityOrAssuranceImprovements,
}
