using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes.Enums;

public enum ModernMethodsConstructionCategoriesType
{
    Undefined = 0,

    [Description("Category 1: Pre-manufacturing: 3D primary structural systems")]
    PreManufacturing3DPrimaryStructuralSystems,

    [Description("Category 2: Pre-manufacturing: 2D primary structural systems")]
    PreManufacturing2DPrimaryStructuralSystems,

    [Description("Category 3: Pre-manufactured component: Non-systemised primary structure")]
    PreManufacturedComponentNonSystemizedPrimaryStructure,

    [Description("Category 4: Additive manufacturing: structuring and non-structural")]
    AdditiveManufacturingStructuringAndNonStructural,

    [Description("Category 5: Pre-manufacturing: Non-structural assemblies and sub-assemblies")]
    PreManufacturingNonStructuralAssembliesAndSubAssemblies,

    [Description("Category 6: Traditional building product led site labour reduction/productivity improvements")]
    TraditionalBuildingProductLedSiteLabourReductionOrProductivityImprovements,

    [Description("Category 7: Site process led labour reduction/productivity/assurance improvements")]
    SiteProcessLedLabourReductionOrProductivityOrAssuranceImprovements,
}
