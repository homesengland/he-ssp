﻿using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record FullHomeType(
    HomeTypeId Id,
    string Name,
    AhpApplicationId ApplicationId,
    string ApplicationName,
    Tenure Tenure,
    bool IsApplicationLocked,
    HousingType HousingType,
    bool IsCompleted,
    bool IsReadOnly,
    OlderPeopleHomeTypeDetails? OlderPeople,
    DisabledPeopleHomeTypeDetails? DisabledPeople,
    DesignPlans? DesignPlans,
    SupportedHousingInformation? SupportedHousing,
    HomeInformation HomeInformation,
    TenureDetails TenureDetails,
    ModernMethodsConstruction ModernMethodsConstruction);
