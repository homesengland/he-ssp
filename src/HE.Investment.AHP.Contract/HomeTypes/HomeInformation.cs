using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

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
        AccessibilityCategoryType AccessibilityCategory)
    : HomeTypeSegmentBase(ApplicationName, HomeTypeName);
