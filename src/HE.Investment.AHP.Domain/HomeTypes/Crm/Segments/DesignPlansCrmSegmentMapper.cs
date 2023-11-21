using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public class DesignPlansCrmSegmentMapper : HomeTypeCrmSegmentMapperBase<DesignPlansSegmentEntity>
{
    public DesignPlansCrmSegmentMapper()
        : base(new[] { nameof(invln_HomeType.invln_designplancomments), nameof(invln_HomeType.invln_happiprinciples) })
    {
    }

    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.DesignPlans;

    public override IHomeTypeSegmentEntity MapToEntity(ApplicationBasicInfo application, HomeTypeDto dto)
    {
        // TODO: map comments when available
        return new DesignPlansSegmentEntity(application, dto.designPrinciples.Select(MapDesignPrinciple));
    }

    protected override DesignPlansSegmentEntity GetSegment(HomeTypeEntity entity) => entity.DesignPlans;

    protected override void MapToDto(HomeTypeDto dto, DesignPlansSegmentEntity segment)
    {
        // TODO: map comments when available
        dto.designPrinciples = segment.DesignPrinciples.Select(MapDesignPrinciple).ToList();
    }

    private static int MapDesignPrinciple(HappiDesignPrincipleType value)
    {
        return value switch
        {
            HappiDesignPrincipleType.AdaptabilityAndCareReadyDesign => (int)invln_happiprinciples.AAdaptabilityandcarereadydesign,
            HappiDesignPrincipleType.BalconiesAndOutdoorSpace => (int)invln_happiprinciples.BBalconiesandoutdoorspace,
            HappiDesignPrincipleType.DaylightInTheHomeAndInSharedSpaces => (int)invln_happiprinciples.CDaylightinthehomeandinsharedspaces,
            HappiDesignPrincipleType.EnergyEfficiencyAndSustainableDesign => (int)invln_happiprinciples.DEnergyefficiencyandsustainabledesign,
            HappiDesignPrincipleType.ExternalSharedSurfacedAndHomeZones => (int)invln_happiprinciples.EExternalsharedsurfacesandhomezones,
            HappiDesignPrincipleType.PlantsTreesAndTheNaturalEnvironment => (int)invln_happiprinciples.FPlantstreesandthenaturalenvironment,
            HappiDesignPrincipleType.PositiveUseOfCirculationSpace => (int)invln_happiprinciples.GPositiveuseofcirculationspace,
            HappiDesignPrincipleType.SharedFacilitiesAndHubs => (int)invln_happiprinciples.HSharedfacilitiesandhubs,
            HappiDesignPrincipleType.SpaceAndFlexibility => (int)invln_happiprinciples.ISpaceandflexibility,
            HappiDesignPrincipleType.StorageForBelongingsAndBicycles => (int)invln_happiprinciples.JStorageforbelongingsandbicycles,
            HappiDesignPrincipleType.NoneOfThese => (int)invln_happiprinciples.KNone,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }

    private static HappiDesignPrincipleType MapDesignPrinciple(int value)
    {
        return value switch
        {
            (int)invln_happiprinciples.AAdaptabilityandcarereadydesign => HappiDesignPrincipleType.AdaptabilityAndCareReadyDesign,
            (int)invln_happiprinciples.BBalconiesandoutdoorspace => HappiDesignPrincipleType.BalconiesAndOutdoorSpace,
            (int)invln_happiprinciples.CDaylightinthehomeandinsharedspaces => HappiDesignPrincipleType.DaylightInTheHomeAndInSharedSpaces,
            (int)invln_happiprinciples.DEnergyefficiencyandsustainabledesign => HappiDesignPrincipleType.EnergyEfficiencyAndSustainableDesign,
            (int)invln_happiprinciples.EExternalsharedsurfacesandhomezones => HappiDesignPrincipleType.ExternalSharedSurfacedAndHomeZones,
            (int)invln_happiprinciples.FPlantstreesandthenaturalenvironment => HappiDesignPrincipleType.PlantsTreesAndTheNaturalEnvironment,
            (int)invln_happiprinciples.GPositiveuseofcirculationspace => HappiDesignPrincipleType.PositiveUseOfCirculationSpace,
            (int)invln_happiprinciples.HSharedfacilitiesandhubs => HappiDesignPrincipleType.SharedFacilitiesAndHubs,
            (int)invln_happiprinciples.ISpaceandflexibility => HappiDesignPrincipleType.SpaceAndFlexibility,
            (int)invln_happiprinciples.JStorageforbelongingsandbicycles => HappiDesignPrincipleType.StorageForBelongingsAndBicycles,
            (int)invln_happiprinciples.KNone => HappiDesignPrincipleType.NoneOfThese,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }
}
