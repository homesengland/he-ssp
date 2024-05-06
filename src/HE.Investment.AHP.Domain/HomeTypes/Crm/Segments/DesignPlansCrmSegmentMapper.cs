using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public class DesignPlansCrmSegmentMapper : HomeTypeCrmSegmentMapperBase<DesignPlansSegmentEntity>
{
    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.DesignPlans;

    public override IHomeTypeSegmentEntity MapToEntity(
        ApplicationBasicInfo application,
        SiteBasicInfo site,
        HomeTypeDto dto,
        IReadOnlyCollection<UploadedFile> uploadedFiles)
    {
        return new DesignPlansSegmentEntity(
            application,
            dto.designPrinciples.Select(MapDesignPrinciple),
            MoreInformation.Create(dto.designPlansMoreInformation),
            uploadedFiles);
    }

    protected override DesignPlansSegmentEntity GetSegment(HomeTypeEntity entity) => entity.DesignPlans;

    protected override void MapToDto(HomeTypeDto dto, DesignPlansSegmentEntity segment)
    {
        dto.designPlansMoreInformation = segment.MoreInformation?.Value;
        dto.designPrinciples = segment.DesignPrinciples.Select(MapDesignPrinciple).ToList();
    }

    private static int MapDesignPrinciple(HappiDesignPrincipleType value)
    {
        return value switch
        {
            HappiDesignPrincipleType.AdaptabilityAndCareReadyDesign => (int)invln_HAPPIprinciples.AAdaptabilityandcarereadydesign,
            HappiDesignPrincipleType.BalconiesAndOutdoorSpace => (int)invln_HAPPIprinciples.BBalconiesandoutdoorspace,
            HappiDesignPrincipleType.DaylightInTheHomeAndInSharedSpaces => (int)invln_HAPPIprinciples.CDaylightinthehomeandinsharedspaces,
            HappiDesignPrincipleType.EnergyEfficiencyAndSustainableDesign => (int)invln_HAPPIprinciples.DEnergyefficiencyandsustainabledesign,
            HappiDesignPrincipleType.ExternalSharedSurfacedAndHomeZones => (int)invln_HAPPIprinciples.EExternalsharedsurfacesandhomezones,
            HappiDesignPrincipleType.PlantsTreesAndTheNaturalEnvironment => (int)invln_HAPPIprinciples.FPlantstreesandthenaturalenvironment,
            HappiDesignPrincipleType.PositiveUseOfCirculationSpace => (int)invln_HAPPIprinciples.GPositiveuseofcirculationspace,
            HappiDesignPrincipleType.SharedFacilitiesAndHubs => (int)invln_HAPPIprinciples.HSharedfacilitiesandhubs,
            HappiDesignPrincipleType.SpaceAndFlexibility => (int)invln_HAPPIprinciples.ISpaceandflexibility,
            HappiDesignPrincipleType.StorageForBelongingsAndBicycles => (int)invln_HAPPIprinciples.JStorageforbelongingsandbicycles,
            HappiDesignPrincipleType.NoneOfThese => (int)invln_HAPPIprinciples.KNone,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }

    private static HappiDesignPrincipleType MapDesignPrinciple(int value)
    {
        return value switch
        {
            (int)invln_HAPPIprinciples.AAdaptabilityandcarereadydesign => HappiDesignPrincipleType.AdaptabilityAndCareReadyDesign,
            (int)invln_HAPPIprinciples.BBalconiesandoutdoorspace => HappiDesignPrincipleType.BalconiesAndOutdoorSpace,
            (int)invln_HAPPIprinciples.CDaylightinthehomeandinsharedspaces => HappiDesignPrincipleType.DaylightInTheHomeAndInSharedSpaces,
            (int)invln_HAPPIprinciples.DEnergyefficiencyandsustainabledesign => HappiDesignPrincipleType.EnergyEfficiencyAndSustainableDesign,
            (int)invln_HAPPIprinciples.EExternalsharedsurfacesandhomezones => HappiDesignPrincipleType.ExternalSharedSurfacedAndHomeZones,
            (int)invln_HAPPIprinciples.FPlantstreesandthenaturalenvironment => HappiDesignPrincipleType.PlantsTreesAndTheNaturalEnvironment,
            (int)invln_HAPPIprinciples.GPositiveuseofcirculationspace => HappiDesignPrincipleType.PositiveUseOfCirculationSpace,
            (int)invln_HAPPIprinciples.HSharedfacilitiesandhubs => HappiDesignPrincipleType.SharedFacilitiesAndHubs,
            (int)invln_HAPPIprinciples.ISpaceandflexibility => HappiDesignPrincipleType.SpaceAndFlexibility,
            (int)invln_HAPPIprinciples.JStorageforbelongingsandbicycles => HappiDesignPrincipleType.StorageForBelongingsAndBicycles,
            (int)invln_HAPPIprinciples.KNone => HappiDesignPrincipleType.NoneOfThese,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }
}
