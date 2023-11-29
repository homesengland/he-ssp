using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public class HomeInformationCrmSegmentMapper : HomeTypeCrmSegmentMapperBase<HomeInformationSegmentEntity>
{
    public HomeInformationCrmSegmentMapper()
        : base(new[]
        {
            nameof(invln_HomeType.invln_numberofhomeshometype),
            nameof(invln_HomeType.invln_numberofbedrooms),
            nameof(invln_HomeType.invln_maxoccupancy),
            nameof(invln_HomeType.invln_numberofstoreys),
            nameof(invln_HomeType.invln_homesusedformoveonaccommodation),
            nameof(invln_HomeType.invln_homesdesignedforuseofparticular),
            nameof(invln_HomeType.invln_buildingtype),
            nameof(invln_HomeType.invln_custombuild),
            nameof(invln_HomeType.invln_facilities),
        })
    {
    }

    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.HomeInformation;

    public override IHomeTypeSegmentEntity MapToEntity(ApplicationBasicInfo application, HomeTypeDto dto)
    {
        return new HomeInformationSegmentEntity(
            dto.numberOfHomes.IsProvided() ? new NumberOfHomes(dto.numberOfHomes!.Value) : null,
            dto.numberOfBedrooms.IsProvided() ? new NumberOfBedrooms(dto.numberOfBedrooms!.Value) : null,
            dto.maxOccupancy.IsProvided() ? new MaximumOccupancy(dto.maxOccupancy!.Value) : null,
            dto.numberOfStoreys.IsProvided() ? new NumberOfStoreys(dto.numberOfStoreys!.Value) : null,
            YesNoTypeMapper.Map(dto.isMoveOnAccommodation),
            MapPeopleGroupForSpecificDesignFeatures(dto.homesDesignedForUseOfParticularGroup),
            MapBuildingType(dto.buildingType),
            YesNoTypeMapper.Map(dto.areHomesCustomBuild),
            MapFacilityType(dto.sharedFacilities));
    }

    protected override HomeInformationSegmentEntity GetSegment(HomeTypeEntity entity) => entity.HomeInformation;

    protected override void MapToDto(HomeTypeDto dto, HomeInformationSegmentEntity segment)
    {
        dto.numberOfHomes = segment.NumberOfHomes?.Value;
        dto.numberOfBedrooms = segment.NumberOfBedrooms?.Value;
        dto.maxOccupancy = segment.MaximumOccupancy?.Value;
        dto.numberOfStoreys = segment.NumberOfStoreys?.Value;
        dto.isMoveOnAccommodation = YesNoTypeMapper.Map(segment.IntendedAsMoveOnAccommodation);
        dto.homesDesignedForUseOfParticularGroup = MapPeopleGroupForSpecificDesignFeatures(segment.PeopleGroupForSpecificDesignFeatures);
        dto.buildingType = MapBuildingType(segment.BuildingType);
        dto.areHomesCustomBuild = YesNoTypeMapper.Map(segment.CustomBuild);
        dto.sharedFacilities = MapFacilityType(segment.FacilityType);
    }

    private static int? MapPeopleGroupForSpecificDesignFeatures(PeopleGroupForSpecificDesignFeaturesType peopleGroupForSpecificDesignFeatures)
    {
        return peopleGroupForSpecificDesignFeatures switch
        {
            PeopleGroupForSpecificDesignFeaturesType.EthnicMinority => (int)invln_homesdesignedforuseofparticulargrou.Peoplefromethnicminoritybackground,
            PeopleGroupForSpecificDesignFeaturesType.DisabledPeople => (int)invln_homesdesignedforuseofparticulargrou.Disabledpeople,
            PeopleGroupForSpecificDesignFeaturesType.FaithGroups => (int)invln_homesdesignedforuseofparticulargrou.Faithgroups,
            PeopleGroupForSpecificDesignFeaturesType.PeopleAtRiskOfDomesticViolence => (int)invln_homesdesignedforuseofparticulargrou.Peopleatriskofdomesticviolence,
            PeopleGroupForSpecificDesignFeaturesType.YoungPeople => (int)invln_homesdesignedforuseofparticulargrou.Youngpeople,
            PeopleGroupForSpecificDesignFeaturesType.OlderPeople => (int)invln_homesdesignedforuseofparticulargrou.Olderpeople,
            PeopleGroupForSpecificDesignFeaturesType.NoneOfThese => (int)invln_homesdesignedforuseofparticulargrou.Noneoftheabove,
            _ => null,
        };
    }

    private static PeopleGroupForSpecificDesignFeaturesType MapPeopleGroupForSpecificDesignFeatures(int? peopleGroupForSpecificDesignFeatures)
    {
        return peopleGroupForSpecificDesignFeatures switch
        {
            (int)invln_homesdesignedforuseofparticulargrou.Peoplefromethnicminoritybackground => PeopleGroupForSpecificDesignFeaturesType.EthnicMinority,
            (int)invln_homesdesignedforuseofparticulargrou.Disabledpeople => PeopleGroupForSpecificDesignFeaturesType.DisabledPeople,
            (int)invln_homesdesignedforuseofparticulargrou.Faithgroups => PeopleGroupForSpecificDesignFeaturesType.FaithGroups,
            (int)invln_homesdesignedforuseofparticulargrou.Peopleatriskofdomesticviolence => PeopleGroupForSpecificDesignFeaturesType.PeopleAtRiskOfDomesticViolence,
            (int)invln_homesdesignedforuseofparticulargrou.Youngpeople => PeopleGroupForSpecificDesignFeaturesType.YoungPeople,
            (int)invln_homesdesignedforuseofparticulargrou.Olderpeople => PeopleGroupForSpecificDesignFeaturesType.OlderPeople,
            (int)invln_homesdesignedforuseofparticulargrou.Noneoftheabove => PeopleGroupForSpecificDesignFeaturesType.NoneOfThese,
            _ => PeopleGroupForSpecificDesignFeaturesType.Undefined,
        };
    }

    private static int? MapBuildingType(BuildingType buildingType)
    {
        return buildingType switch
        {
            BuildingType.House => (int)invln_buildingtype.House,
            BuildingType.Flat => (int)invln_buildingtype.Flat,
            BuildingType.Bedsit => (int)invln_buildingtype.Bedsit,
            BuildingType.Bungalow => (int)invln_buildingtype.Bungalow,
            BuildingType.Maisonette => (int)invln_buildingtype.Maisonette,
            _ => null,

        };
    }

    private static BuildingType MapBuildingType(int? buildingType)
    {
        return buildingType switch
        {
            (int)invln_buildingtype.House => BuildingType.House,
            (int)invln_buildingtype.Flat => BuildingType.Flat,
            (int)invln_buildingtype.Bedsit => BuildingType.Bedsit,
            (int)invln_buildingtype.Bungalow => BuildingType.Bungalow,
            (int)invln_buildingtype.Maisonette => BuildingType.Maisonette,
            _ => BuildingType.Undefined,
        };
    }

    private static int? MapFacilityType(FacilityType facilityType)
    {
        return facilityType switch
        {
            FacilityType.SelfContainedFacilities => (int)invln_facilities.Selfcontainedfacilities,
            FacilityType.SharedFacilities => (int)invln_facilities.Sharedfacilities,
            FacilityType.MixOfSelfContainedAndSharedFacilities => (int)invln_facilities.Mixofselfcontainedandsharedfacilities,
            _ => null,

        };
    }

    private static FacilityType MapFacilityType(int? facilityType)
    {
        return facilityType switch
        {
            (int)invln_facilities.Selfcontainedfacilities => FacilityType.SelfContainedFacilities,
            (int)invln_facilities.Sharedfacilities => FacilityType.SharedFacilities,
            (int)invln_facilities.Mixofselfcontainedandsharedfacilities => FacilityType.MixOfSelfContainedAndSharedFacilities,
            _ => FacilityType.Undefined,
        };
    }
}
