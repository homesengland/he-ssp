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
            nameof(invln_HomeType.invln_iswheelchairstandardmet),
            nameof(invln_HomeType.invln_accessibilitycategory),
            nameof(invln_HomeType.invln_floorarea),
            nameof(invln_HomeType.invln_doallhomesmeetNDSS),
            nameof(invln_HomeType.invln_whichndssstandardshavebeenmet),
        })
    {
    }

    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.HomeInformation;

    public override IHomeTypeSegmentEntity MapToEntity(
        ApplicationBasicInfo application,
        SiteBasicInfo site,
        HomeTypeDto dto,
        IReadOnlyCollection<UploadedFile> uploadedFiles)
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
            MapFacilityType(dto.sharedFacilities),
            YesNoTypeMapper.Map(dto.isWheelchairStandardMet),
            MapAccessibilityCategory(dto.accessibilityCategory),
            dto.floorArea.IsProvided() ? new FloorArea(dto.floorArea!.Value) : null,
            YesNoTypeMapper.Map(dto.doAllHomesMeetNDSS),
            dto.whichNDSSStandardsHaveBeenMet.Select(MapNationallyDescribedSpaceStandards));
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
        dto.isWheelchairStandardMet = YesNoTypeMapper.Map(segment.AccessibilityStandards);
        dto.accessibilityCategory = MapAccessibilityCategory(segment.AccessibilityCategory);
        dto.floorArea = segment.InternalFloorArea?.Value;
        dto.doAllHomesMeetNDSS = YesNoTypeMapper.Map(segment.MeetNationallyDescribedSpaceStandards);
        dto.whichNDSSStandardsHaveBeenMet = segment.NationallyDescribedSpaceStandards.Select(MapNationallyDescribedSpaceStandards).ToList();
    }

    private static int? MapPeopleGroupForSpecificDesignFeatures(PeopleGroupForSpecificDesignFeaturesType peopleGroupForSpecificDesignFeatures)
    {
        return peopleGroupForSpecificDesignFeatures switch
        {
            PeopleGroupForSpecificDesignFeaturesType.EthnicMinority => (int)invln_Homesdesignedforuseofparticulargrou.Peoplefromethnicminoritybackground,
            PeopleGroupForSpecificDesignFeaturesType.DisabledPeople => (int)invln_Homesdesignedforuseofparticulargrou.Disabledpeople,
            PeopleGroupForSpecificDesignFeaturesType.FaithGroups => (int)invln_Homesdesignedforuseofparticulargrou.Faithgroups,
            PeopleGroupForSpecificDesignFeaturesType.PeopleAtRiskOfDomesticViolence => (int)invln_Homesdesignedforuseofparticulargrou.Peopleatriskofdomesticviolence,
            PeopleGroupForSpecificDesignFeaturesType.YoungPeople => (int)invln_Homesdesignedforuseofparticulargrou.Youngpeople,
            PeopleGroupForSpecificDesignFeaturesType.OlderPeople => (int)invln_Homesdesignedforuseofparticulargrou.Olderpeople,
            PeopleGroupForSpecificDesignFeaturesType.NoneOfThese => (int)invln_Homesdesignedforuseofparticulargrou.Noneoftheabove,
            _ => null,
        };
    }

    private static PeopleGroupForSpecificDesignFeaturesType MapPeopleGroupForSpecificDesignFeatures(int? peopleGroupForSpecificDesignFeatures)
    {
        return peopleGroupForSpecificDesignFeatures switch
        {
            (int)invln_Homesdesignedforuseofparticulargrou.Peoplefromethnicminoritybackground => PeopleGroupForSpecificDesignFeaturesType.EthnicMinority,
            (int)invln_Homesdesignedforuseofparticulargrou.Disabledpeople => PeopleGroupForSpecificDesignFeaturesType.DisabledPeople,
            (int)invln_Homesdesignedforuseofparticulargrou.Faithgroups => PeopleGroupForSpecificDesignFeaturesType.FaithGroups,
            (int)invln_Homesdesignedforuseofparticulargrou.Peopleatriskofdomesticviolence => PeopleGroupForSpecificDesignFeaturesType.PeopleAtRiskOfDomesticViolence,
            (int)invln_Homesdesignedforuseofparticulargrou.Youngpeople => PeopleGroupForSpecificDesignFeaturesType.YoungPeople,
            (int)invln_Homesdesignedforuseofparticulargrou.Olderpeople => PeopleGroupForSpecificDesignFeaturesType.OlderPeople,
            (int)invln_Homesdesignedforuseofparticulargrou.Noneoftheabove => PeopleGroupForSpecificDesignFeaturesType.NoneOfThese,
            _ => PeopleGroupForSpecificDesignFeaturesType.Undefined,
        };
    }

    private static int? MapBuildingType(BuildingType buildingType)
    {
        return buildingType switch
        {
            BuildingType.House => (int)invln_Buildingtype.House,
            BuildingType.Flat => (int)invln_Buildingtype.Flat,
            BuildingType.Bedsit => (int)invln_Buildingtype.Bedsit,
            BuildingType.Bungalow => (int)invln_Buildingtype.Bungalow,
            BuildingType.Maisonette => (int)invln_Buildingtype.Maisonette,
            _ => null,

        };
    }

    private static BuildingType MapBuildingType(int? buildingType)
    {
        return buildingType switch
        {
            (int)invln_Buildingtype.House => BuildingType.House,
            (int)invln_Buildingtype.Flat => BuildingType.Flat,
            (int)invln_Buildingtype.Bedsit => BuildingType.Bedsit,
            (int)invln_Buildingtype.Bungalow => BuildingType.Bungalow,
            (int)invln_Buildingtype.Maisonette => BuildingType.Maisonette,
            _ => BuildingType.Undefined,
        };
    }

    private static int? MapFacilityType(FacilityType facilityType)
    {
        return facilityType switch
        {
            FacilityType.SelfContainedFacilities => (int)invln_Facilities.Selfcontainedfacilities,
            FacilityType.SharedFacilities => (int)invln_Facilities.Sharedfacilities,
            FacilityType.MixOfSelfContainedAndSharedFacilities => (int)invln_Facilities.Mixofselfcontainedandsharedfacilities,
            _ => null,

        };
    }

    private static FacilityType MapFacilityType(int? facilityType)
    {
        return facilityType switch
        {
            (int)invln_Facilities.Selfcontainedfacilities => FacilityType.SelfContainedFacilities,
            (int)invln_Facilities.Sharedfacilities => FacilityType.SharedFacilities,
            (int)invln_Facilities.Mixofselfcontainedandsharedfacilities => FacilityType.MixOfSelfContainedAndSharedFacilities,
            _ => FacilityType.Undefined,
        };
    }

    private static int? MapAccessibilityCategory(AccessibilityCategoryType accessibilityCategory)
    {
        return accessibilityCategory switch
        {
            AccessibilityCategoryType.VisitableDwellings => (int)invln_AccessibilitycategorySet.Category1VisitableDwelling,
            AccessibilityCategoryType.AccessibleAndAdaptableDwellings => (int)invln_AccessibilitycategorySet.Category2Accessibleandacceptabledwelling,
            AccessibilityCategoryType.WheelchairUserDwellings => (int)invln_AccessibilitycategorySet.Category3Wheelchairuserdwellings,
            _ => null,
        };
    }

    private static AccessibilityCategoryType MapAccessibilityCategory(int? accessibilityCategory)
    {
        return accessibilityCategory switch
        {
            (int)invln_AccessibilitycategorySet.Category1VisitableDwelling => AccessibilityCategoryType.VisitableDwellings,
            (int)invln_AccessibilitycategorySet.Category2Accessibleandacceptabledwelling => AccessibilityCategoryType.AccessibleAndAdaptableDwellings,
            (int)invln_AccessibilitycategorySet.Category3Wheelchairuserdwellings => AccessibilityCategoryType.WheelchairUserDwellings,
            _ => AccessibilityCategoryType.Undefined,
        };
    }

    private static int MapNationallyDescribedSpaceStandards(NationallyDescribedSpaceStandardType value)
    {
        return value switch
        {
            NationallyDescribedSpaceStandardType.BuiltInStorageSpaceSize => (int)invln_WhichNDSSstandardshavebeenmet.Builtinstoragespacesize,
            NationallyDescribedSpaceStandardType.BedroomAreas => (int)invln_WhichNDSSstandardshavebeenmet.Bedroomareas,
            NationallyDescribedSpaceStandardType.BedroomWidth => (int)invln_WhichNDSSstandardshavebeenmet.Bedroomwidths,
            NationallyDescribedSpaceStandardType.NoneOfThese => (int)invln_WhichNDSSstandardshavebeenmet.Noneofthese,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }

    private static NationallyDescribedSpaceStandardType MapNationallyDescribedSpaceStandards(int value)
    {
        return value switch
        {
            (int)invln_WhichNDSSstandardshavebeenmet.Builtinstoragespacesize => NationallyDescribedSpaceStandardType.BuiltInStorageSpaceSize,
            (int)invln_WhichNDSSstandardshavebeenmet.Bedroomareas => NationallyDescribedSpaceStandardType.BedroomAreas,
            (int)invln_WhichNDSSstandardshavebeenmet.Bedroomwidths => NationallyDescribedSpaceStandardType.BedroomWidth,
            (int)invln_WhichNDSSstandardshavebeenmet.Noneofthese => NationallyDescribedSpaceStandardType.NoneOfThese,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is not supported by CRM mapping."),
        };
    }
}
