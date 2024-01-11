using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveDisabledPeopleHousingTypeCommand(AhpApplicationId ApplicationId, string HomeTypeId, DisabledPeopleHousingType HousingType)
    : ISaveHomeTypeSegmentCommand;
