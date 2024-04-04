using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record FullHomeType(
    ApplicationDetails Application,
    HomeTypeId Id,
    string Name,
    HousingType HousingType,
    bool IsCompleted,
    OlderPeopleHomeTypeDetails? OlderPeople,
    DisabledPeopleHomeTypeDetails? DisabledPeople,
    DesignPlans? DesignPlans,
    SupportedHousingInformation? SupportedHousing,
    HomeInformation HomeInformation,
    TenureDetails TenureDetails,
    ModernMethodsConstruction ModernMethodsConstruction);
