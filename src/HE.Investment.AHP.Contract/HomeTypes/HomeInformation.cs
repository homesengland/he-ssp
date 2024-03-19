using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record HomeInformation(
        string ApplicationName,
        string HomeTypeName,
        int? NumberOfHomes,
        int? NumberOfBedrooms,
        int? MaximumOccupancy,
        int? NumberOfStoreys,
        YesNoType IntendedAsMoveOnAccommodation,
        PeopleGroupForSpecificDesignFeaturesType PeopleGroupForSpecificDesignFeatures,
        BuildingType BuildingType,
        YesNoType CustomBuild,
        FacilityType FacilityType,
        YesNoType AccessibilityStandards,
        AccessibilityCategoryType AccessibilityCategory,
        decimal? InternalFloorArea,
        YesNoType MeetNationallyDescribedSpaceStandards,
        IList<NationallyDescribedSpaceStandardType> NationallyDescribedSpaceStandards)
    : HomeTypeSegmentBase(ApplicationName, HomeTypeName);
