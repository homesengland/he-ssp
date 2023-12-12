using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record FullHomeType(
    string Id,
    string Name,
    string ApplicationId,
    string ApplicationName,
    Tenure Tenure,
    HousingType HousingType,
    bool IsCompleted,
    OlderPeopleHomeTypeDetails? OlderPeople,
    DisabledPeopleHomeTypeDetails? DisabledPeople,
    DesignPlans? DesignPlans,
    SupportedHousingInformation? SupportedHousing,
    HomeInformation HomeInformation,
    TenureDetails TenureDetails);
