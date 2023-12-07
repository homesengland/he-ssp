using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record DisabledPeopleHomeTypeDetails(
        string ApplicationName,
        string HomeTypeName,
        DisabledPeopleHousingType HousingType,
        DisabledPeopleClientGroupType ClientGroupType)
    : HomeTypeSegmentBase(ApplicationName, HomeTypeName);
