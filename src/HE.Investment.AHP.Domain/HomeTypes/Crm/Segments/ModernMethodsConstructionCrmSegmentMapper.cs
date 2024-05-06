using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public class ModernMethodsConstructionCrmSegmentMapper : HomeTypeCrmSegmentMapperBase<ModernMethodsConstructionSegmentEntity>
{
    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.ModernMethodsConstruction;

    public override IHomeTypeSegmentEntity MapToEntity(
        ApplicationBasicInfo application,
        SiteBasicInfo site,
        HomeTypeDto dto,
        IReadOnlyCollection<UploadedFile> uploadedFiles)
    {
        return new ModernMethodsConstructionSegmentEntity(
            site.SiteUsingModernMethodsOfConstruction,
            YesNoTypeMapper.Map(dto.mmcApplied),
            dto.mmcCategories.Select(MapMmcCategories),
            dto.mmcCategories2Subcategories.Select(MapMmcCategory2Subcategories),
            dto.mmcCategories1Subcategories.Select(MapMmcCategory1Subcategories));
    }

    protected override ModernMethodsConstructionSegmentEntity GetSegment(HomeTypeEntity entity) => entity.ModernMethodsConstruction;

    protected override void MapToDto(HomeTypeDto dto, ModernMethodsConstructionSegmentEntity segment)
    {
        dto.mmcApplied = YesNoTypeMapper.Map(segment.ModernMethodsConstructionApplied);
        dto.mmcCategories = segment.ModernMethodsConstructionCategories.Select(MapMmcCategories).ToList();
        dto.mmcCategories1Subcategories = segment.ModernMethodsConstruction3DSubcategories.Select(MapMmcCategory1Subcategories).ToList();
        dto.mmcCategories2Subcategories = segment.ModernMethodsConstruction2DSubcategories.Select(MapMmcCategory2Subcategories).ToList();
    }

    private static int MapMmcCategories(ModernMethodsConstructionCategoriesType value)
    {
        return value switch
        {
            ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems => (int)invln_MMCCategories
                .Category1Premanufacturing3Dprimarystructuralsystems,
            ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems => (int)invln_MMCCategories
                .Category2Premanufacturing2Dprimarystructuralsystems,
            ModernMethodsConstructionCategoriesType.Category3PreManufacturedComponentNonSystemizedPrimaryStructure => (int)invln_MMCCategories
                .Category3PremanufacturedcomponentsNonsystemisedprimarystructure,
            ModernMethodsConstructionCategoriesType.Category4AdditiveManufacturingStructuringAndNonStructural => (int)invln_MMCCategories
                .Category4AdditivemanufacturingStructuralandnonstructural,
            ModernMethodsConstructionCategoriesType.Category5PreManufacturingNonStructuralAssembliesAndSubAssemblies => (int)invln_MMCCategories
                .Category5PremanufacturingNonstructuralassembliesandsubassemblies,
            ModernMethodsConstructionCategoriesType.Category6TraditionalBuildingProductLedSiteLabourReductionOrProductivityImprovements =>
                (int)invln_MMCCategories.Category6Traditionalbuildingproductledsitelabourreductionproductivityimprovements,
            ModernMethodsConstructionCategoriesType.Category7SiteProcessLedLabourReductionOrProductivityOrAssuranceImprovements => (int)invln_MMCCategories
                .Category7Siteprocessledlabourreductionproductivityassuranceimprovements,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }

    private static ModernMethodsConstructionCategoriesType MapMmcCategories(int value)
    {
        return value switch
        {
            (int)invln_MMCCategories.Category1Premanufacturing3Dprimarystructuralsystems => ModernMethodsConstructionCategoriesType
                .Category1PreManufacturing3DPrimaryStructuralSystems,
            (int)invln_MMCCategories.Category2Premanufacturing2Dprimarystructuralsystems => ModernMethodsConstructionCategoriesType
                .Category2PreManufacturing2DPrimaryStructuralSystems,
            (int)invln_MMCCategories.Category3PremanufacturedcomponentsNonsystemisedprimarystructure => ModernMethodsConstructionCategoriesType
                .Category3PreManufacturedComponentNonSystemizedPrimaryStructure,
            (int)invln_MMCCategories.Category4AdditivemanufacturingStructuralandnonstructural => ModernMethodsConstructionCategoriesType
                .Category4AdditiveManufacturingStructuringAndNonStructural,
            (int)invln_MMCCategories.Category5PremanufacturingNonstructuralassembliesandsubassemblies => ModernMethodsConstructionCategoriesType
                .Category5PreManufacturingNonStructuralAssembliesAndSubAssemblies,
            (int)invln_MMCCategories.Category6Traditionalbuildingproductledsitelabourreductionproductivityimprovements =>
                ModernMethodsConstructionCategoriesType.Category6TraditionalBuildingProductLedSiteLabourReductionOrProductivityImprovements,
            (int)invln_MMCCategories.Category7Siteprocessledlabourreductionproductivityassuranceimprovements => ModernMethodsConstructionCategoriesType
                .Category7SiteProcessLedLabourReductionOrProductivityOrAssuranceImprovements,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }

    private static int MapMmcCategory1Subcategories(ModernMethodsConstruction3DSubcategoriesType value)
    {
        return value switch
        {
            ModernMethodsConstruction3DSubcategoriesType.StructuralChassisOnly => (int)invln_MMCcategory1subcategories._1astructuralchassisonlynotfittedout,
            ModernMethodsConstruction3DSubcategoriesType.StructuralChassisAndInternallyFittedOut =>
                (int)invln_MMCcategory1subcategories._1bstructuralchassisandinternalfitout,
            ModernMethodsConstruction3DSubcategoriesType.StructuralChassisInternallyFittedOutAndExternalCladdingOrRoofingCompleted =>
                (int)invln_MMCcategory1subcategories._1cstructuralchassisfitoutandexternalcladdingroofingcomplete,
            ModernMethodsConstruction3DSubcategoriesType.StructuralChassisInternallyFittedOutAndPoddedRoomAssembled =>
                (int)invln_MMCcategory1subcategories._1dstructuralchassisandinternalfitoutpoddedroomassemblesbathroomskitchensetc,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }

    private static ModernMethodsConstruction3DSubcategoriesType MapMmcCategory1Subcategories(int value)
    {
        return value switch
        {
            (int)invln_MMCcategory1subcategories._1astructuralchassisonlynotfittedout => ModernMethodsConstruction3DSubcategoriesType
                .StructuralChassisOnly,
            (int)invln_MMCcategory1subcategories._1bstructuralchassisandinternalfitout => ModernMethodsConstruction3DSubcategoriesType
                .StructuralChassisAndInternallyFittedOut,
            (int)invln_MMCcategory1subcategories._1cstructuralchassisfitoutandexternalcladdingroofingcomplete =>
                ModernMethodsConstruction3DSubcategoriesType.StructuralChassisInternallyFittedOutAndExternalCladdingOrRoofingCompleted,
            (int)invln_MMCcategory1subcategories._1dstructuralchassisandinternalfitoutpoddedroomassemblesbathroomskitchensetc =>
                ModernMethodsConstruction3DSubcategoriesType.StructuralChassisInternallyFittedOutAndPoddedRoomAssembled,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }

    private static int MapMmcCategory2Subcategories(ModernMethodsConstruction2DSubcategoriesType value)
    {
        return value switch
        {
            ModernMethodsConstruction2DSubcategoriesType.BasicFramingOnly =>
                (int)invln_MMCcategory2subcategories._2abasicframingonlyinclwallsfloorsstairsroof,
            ModernMethodsConstruction2DSubcategoriesType.EnhancedConsolidation =>
                (int)invln_MMCcategory2subcategories._2benhancedconsolidationinsulationinternalliningsetc,
            ModernMethodsConstruction2DSubcategoriesType.FurtherEnhancedConsolidation =>
                (int)invln_MMCcategory2subcategories._2cfurtherenhancedconsolidationinsulationliningsexternalcladdingroofingdoorswindows,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }

    private static ModernMethodsConstruction2DSubcategoriesType MapMmcCategory2Subcategories(int value)
    {
        return value switch
        {
            (int)invln_MMCcategory2subcategories._2abasicframingonlyinclwallsfloorsstairsroof =>
                ModernMethodsConstruction2DSubcategoriesType.BasicFramingOnly,
            (int)invln_MMCcategory2subcategories._2benhancedconsolidationinsulationinternalliningsetc =>
                ModernMethodsConstruction2DSubcategoriesType.EnhancedConsolidation,
            (int)invln_MMCcategory2subcategories._2cfurtherenhancedconsolidationinsulationliningsexternalcladdingroofingdoorswindows =>
                ModernMethodsConstruction2DSubcategoriesType.FurtherEnhancedConsolidation,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }
}
