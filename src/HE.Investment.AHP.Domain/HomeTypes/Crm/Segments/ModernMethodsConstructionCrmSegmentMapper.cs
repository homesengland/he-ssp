using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public class ModernMethodsConstructionCrmSegmentMapper : HomeTypeCrmSegmentMapperBase<ModernMethodsConstructionEntity>
{
    public ModernMethodsConstructionCrmSegmentMapper()
        : base(new[]
        {
            // todo field missing inc crm nameof(invln_HomeType.invln_mmcapplied),
            nameof(invln_HomeType.invln_mmccategories),
            nameof(invln_HomeType.invln_mmccategory1subcategories),
            nameof(invln_HomeType.invln_mmccategory2subcategories),
        })
    {
    }

    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.ModernMethodsConstruction;

    public override IHomeTypeSegmentEntity MapToEntity(ApplicationBasicInfo application, HomeTypeDto dto, IReadOnlyCollection<UploadedFile> uploadedFiles)
    {
        return new ModernMethodsConstructionEntity();

        // TODO waiting for crm
        // return new ModernMethodsConstructionEntity(
        //     YesNoTypeMapper.Map(dto.mmcApplied),
        //     dto.mmcCategories.Select(MapMmcCategories),
        //     dto.mmcCategory1Subcategories.Select(MapMmcCategory1Subcategories),
        //     dto.mmcCategory2Subcategories.Select(MapMmcCategory2Subcategories));
    }

    protected override ModernMethodsConstructionEntity GetSegment(HomeTypeEntity entity) => entity.ModernMethodsConstruction;

    protected override void MapToDto(HomeTypeDto dto, ModernMethodsConstructionEntity segment)
    {
        // TODO waiting for crm
        // dto.mmcApplied = YesNoTypeMapper.Map(segment.ModernMethodsConstructionApplied);
        // dto.mmcCategories = segment.ModernMethodsConstructionCategories.Select(MapMmcCategories).ToList();
        // dto.mmcCategory1Subcategories = segment.ModernMethodsConstruction3DSubcategories.Select(MapMmcCategory1Subcategories).ToList();
        // dto.mmcCategory2Subcategories = segment.ModernMethodsConstruction2DSubcategories.Select(MapMmcCategory2Subcategories).ToList();
    }

    // TODO temporary disable warning that methods are unused
#pragma warning disable IDE0051
    private static int MapMmcCategories(ModernMethodsConstructionCategoriesType value)
    {
        return value switch
        {
            ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems => (int)invln_mmccategories
                .Category1Premanufacturing3Dprimarystructuralsystems,
            ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems => (int)invln_mmccategories
                .Category2Premanufacturing2Dprimarystructuralsystems,
            ModernMethodsConstructionCategoriesType.Category3PreManufacturedComponentNonSystemizedPrimaryStructure => (int)invln_mmccategories
                .Category3PremanufacturedcomponentsNonsystemisedprimarystructure,
            ModernMethodsConstructionCategoriesType.Category4AdditiveManufacturingStructuringAndNonStructural => (int)invln_mmccategories
                .Category4AdditivemanufacturingStructuralandnonstructural,
            ModernMethodsConstructionCategoriesType.Category5PreManufacturingNonStructuralAssembliesAndSubAssemblies => (int)invln_mmccategories
                .Category5PremanufacturingNonstructuralassembliesandsubassemblies,
            ModernMethodsConstructionCategoriesType.Category6TraditionalBuildingProductLedSiteLabourReductionOrProductivityImprovements =>
                (int)invln_mmccategories.Category6Traditionalbuildingproductledsitelabourreductionproductivityimprovements,
            ModernMethodsConstructionCategoriesType.Category7SiteProcessLedLabourReductionOrProductivityOrAssuranceImprovements => (int)invln_mmccategories
                .Category7Siteprocessledlabourreductionproductivityassuranceimprovements,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }

    private static ModernMethodsConstructionCategoriesType MapMmcCategories(int value)
    {
        return value switch
        {
            (int)invln_mmccategories.Category1Premanufacturing3Dprimarystructuralsystems => ModernMethodsConstructionCategoriesType
                .Category1PreManufacturing3DPrimaryStructuralSystems,
            (int)invln_mmccategories.Category2Premanufacturing2Dprimarystructuralsystems => ModernMethodsConstructionCategoriesType
                .Category2PreManufacturing2DPrimaryStructuralSystems,
            (int)invln_mmccategories.Category3PremanufacturedcomponentsNonsystemisedprimarystructure => ModernMethodsConstructionCategoriesType
                .Category3PreManufacturedComponentNonSystemizedPrimaryStructure,
            (int)invln_mmccategories.Category4AdditivemanufacturingStructuralandnonstructural => ModernMethodsConstructionCategoriesType
                .Category4AdditiveManufacturingStructuringAndNonStructural,
            (int)invln_mmccategories.Category5PremanufacturingNonstructuralassembliesandsubassemblies => ModernMethodsConstructionCategoriesType
                .Category5PreManufacturingNonStructuralAssembliesAndSubAssemblies,
            (int)invln_mmccategories.Category6Traditionalbuildingproductledsitelabourreductionproductivityimprovements =>
                ModernMethodsConstructionCategoriesType.Category6TraditionalBuildingProductLedSiteLabourReductionOrProductivityImprovements,
            (int)invln_mmccategories.Category7Siteprocessledlabourreductionproductivityassuranceimprovements => ModernMethodsConstructionCategoriesType
                .Category7SiteProcessLedLabourReductionOrProductivityOrAssuranceImprovements,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }

    private static int MapMmcCategory1Subcategories(ModernMethodsConstruction3DSubcategoriesType value)
    {
        return value switch
        {
            ModernMethodsConstruction3DSubcategoriesType.StructuralChassisOnly => (int)invln_mmccategory1subcategories
                .Number_1astructuralchassisonlynotfittedout,
            ModernMethodsConstruction3DSubcategoriesType.StructuralChassisAndInternallyFittedOut => (int)invln_mmccategory1subcategories
                .Number_1bstructuralchassisandinternalfitout,
            ModernMethodsConstruction3DSubcategoriesType.StructuralChassisInternallyFittedOutAndExternalCladdingOrRoofingCompleted =>
                (int)invln_mmccategory1subcategories.Number_1cstructuralchassisfitoutandexternalcladdingroofingcomplete,
            ModernMethodsConstruction3DSubcategoriesType.StructuralChassisInternallyFittedOutAndPoddedRoomAssembled => (int)invln_mmccategory1subcategories
                .Number_1dstructuralchassisandinternalfitoutpoddedroomassemblesbathroomskitchensetc,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }

    private static ModernMethodsConstruction3DSubcategoriesType MapMmcCategory1Subcategories(int value)
    {
        return value switch
        {
            (int)invln_mmccategory1subcategories.Number_1astructuralchassisonlynotfittedout => ModernMethodsConstruction3DSubcategoriesType
                .StructuralChassisOnly,
            (int)invln_mmccategory1subcategories.Number_1bstructuralchassisandinternalfitout => ModernMethodsConstruction3DSubcategoriesType
                .StructuralChassisAndInternallyFittedOut,
            (int)invln_mmccategory1subcategories.Number_1cstructuralchassisfitoutandexternalcladdingroofingcomplete =>
                ModernMethodsConstruction3DSubcategoriesType.StructuralChassisInternallyFittedOutAndExternalCladdingOrRoofingCompleted,
            (int)invln_mmccategory1subcategories.Number_1dstructuralchassisandinternalfitoutpoddedroomassemblesbathroomskitchensetc =>
                ModernMethodsConstruction3DSubcategoriesType.StructuralChassisInternallyFittedOutAndPoddedRoomAssembled,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }

    private static int MapMmcCategory2Subcategories(ModernMethodsConstruction2DSubcategoriesType value)
    {
        return value switch
        {
            ModernMethodsConstruction2DSubcategoriesType.BasicFramingOnly => (int)invln_mmccategory2subcategories
                .Number_2abasicframingonlyinclwallsfloorsstairsroof,
            ModernMethodsConstruction2DSubcategoriesType.EnhancedConsolidation => (int)invln_mmccategory2subcategories
                .Number_2benhancedconsolidationinsulationinternalliningsetc,
            ModernMethodsConstruction2DSubcategoriesType.FurtherEnhancedConsolidation => (int)invln_mmccategory2subcategories
                .Number_2cfurtherenhancedconsolidationinsulationliningsexternalcladdingroofingdoorswindows,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }

    private static ModernMethodsConstruction2DSubcategoriesType MapMmcCategory2Subcategories(int value)
    {
        return value switch
        {
            (int)invln_mmccategory2subcategories.Number_2abasicframingonlyinclwallsfloorsstairsroof => ModernMethodsConstruction2DSubcategoriesType
                .BasicFramingOnly,
            (int)invln_mmccategory2subcategories.Number_2benhancedconsolidationinsulationinternalliningsetc => ModernMethodsConstruction2DSubcategoriesType
                .EnhancedConsolidation,
            (int)invln_mmccategory2subcategories.Number_2cfurtherenhancedconsolidationinsulationliningsexternalcladdingroofingdoorswindows =>
                ModernMethodsConstruction2DSubcategoriesType.FurtherEnhancedConsolidation,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }
#pragma warning restore IDE0051
}
