using System.Collections.Concurrent;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public class ModernMethodsConstructionCrmSegmentMapper : HomeTypeCrmSegmentMapperBase<ModernMethodsConstructionSegmentEntity>
{
    private static readonly IDictionary<string, MmcDto> Dtos = new ConcurrentDictionary<string, MmcDto>();

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

    public override IHomeTypeSegmentEntity MapToEntity(
        ApplicationBasicInfo application,
        SiteBasicInfo site,
        HomeTypeDto dto,
        IReadOnlyCollection<UploadedFile> uploadedFiles)
    {
        if (!Dtos.TryGetValue(dto.id, out var mmc))
        {
            if (!Dtos.TryGetValue(string.Empty, out mmc))
            {
                return new ModernMethodsConstructionSegmentEntity(site.SiteUsingModernMethodsOfConstruction);
            }

            Dtos.Remove(string.Empty);
            Dtos.Add(dto.id, mmc);
        }

        return new ModernMethodsConstructionSegmentEntity(
            site.SiteUsingModernMethodsOfConstruction,
            YesNoTypeMapper.Map(mmc.MmcApplied),
            mmc.MmcCategories.Select(MapMmcCategories),
            mmc.MmcCategory2Subcategories.Select(MapMmcCategory2Subcategories),
            mmc.MmcCategory1Subcategories.Select(MapMmcCategory1Subcategories));
    }

    protected override ModernMethodsConstructionSegmentEntity GetSegment(HomeTypeEntity entity) => entity.ModernMethodsConstruction;

    protected override void MapToDto(HomeTypeDto dto, ModernMethodsConstructionSegmentEntity segment)
    {
        if (!Dtos.TryGetValue(dto.id ?? string.Empty, out var mmc))
        {
            mmc = new MmcDto();
            Dtos.Add(dto.id ?? string.Empty, mmc);
        }

        mmc.MmcApplied = YesNoTypeMapper.Map(segment.ModernMethodsConstructionApplied);
        mmc.MmcCategories = segment.ModernMethodsConstructionCategories.Select(MapMmcCategories).ToList();
        mmc.MmcCategory1Subcategories = segment.ModernMethodsConstruction3DSubcategories.Select(MapMmcCategory1Subcategories).ToList();
        mmc.MmcCategory2Subcategories = segment.ModernMethodsConstruction2DSubcategories.Select(MapMmcCategory2Subcategories).ToList();
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

    private sealed class MmcDto
    {
        public MmcDto(
            bool? mmcApplied = null,
            IList<int>? mmcCategories = null,
            IList<int>? mmcCategory1Subcategories = null,
            IList<int>? mmcCategory2Subcategories = null)
        {
            MmcApplied = mmcApplied;
            MmcCategories = mmcCategories ?? new List<int>();
            MmcCategory1Subcategories = mmcCategory1Subcategories ?? new List<int>();
            MmcCategory2Subcategories = mmcCategory2Subcategories ?? new List<int>();
        }

        public bool? MmcApplied { get; set; }

        public IList<int> MmcCategories { get; set; }

        public IList<int> MmcCategory1Subcategories { get; set; }

        public IList<int> MmcCategory2Subcategories { get; set; }
    }
}
