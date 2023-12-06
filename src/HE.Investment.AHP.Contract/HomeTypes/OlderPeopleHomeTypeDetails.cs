using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record OlderPeopleHomeTypeDetails(string ApplicationName, string HomeTypeName, OlderPeopleHousingType HousingType)
    : HomeTypeSegmentBase(ApplicationName, HomeTypeName);
