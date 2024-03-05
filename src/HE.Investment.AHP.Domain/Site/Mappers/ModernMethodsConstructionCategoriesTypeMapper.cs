using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Site.Mappers;

public class ModernMethodsConstructionCategoriesTypeMapper : EnumMapper<ModernMethodsConstructionCategoriesType>
{
    protected override IDictionary<ModernMethodsConstructionCategoriesType, int?> Mapping =>
        new Dictionary<ModernMethodsConstructionCategoriesType, int?>
        {
            { ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems, (int)invln_MMCCategories.Category1Premanufacturing3Dprimarystructuralsystems },
            { ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems, (int)invln_MMCCategories.Category2Premanufacturing2Dprimarystructuralsystems },
            { ModernMethodsConstructionCategoriesType.Category3PreManufacturedComponentNonSystemizedPrimaryStructure, (int)invln_MMCCategories.Category3PremanufacturedcomponentsNonsystemisedprimarystructure },
            { ModernMethodsConstructionCategoriesType.Category4AdditiveManufacturingStructuringAndNonStructural, (int)invln_MMCCategories.Category4AdditivemanufacturingStructuralandnonstructural },
            { ModernMethodsConstructionCategoriesType.Category5PreManufacturingNonStructuralAssembliesAndSubAssemblies, (int)invln_MMCCategories.Category5PremanufacturingNonstructuralassembliesandsubassemblies },
            { ModernMethodsConstructionCategoriesType.Category6TraditionalBuildingProductLedSiteLabourReductionOrProductivityImprovements, (int)invln_MMCCategories.Category6Traditionalbuildingproductledsitelabourreductionproductivityimprovements },
            { ModernMethodsConstructionCategoriesType.Category7SiteProcessLedLabourReductionOrProductivityOrAssuranceImprovements, (int)invln_MMCCategories.Category7Siteprocessledlabourreductionproductivityassuranceimprovements },
        };
}
