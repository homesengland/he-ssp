using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveOlderPeopleHousingTypeCommand(AhpApplicationId ApplicationId, string HomeTypeId, OlderPeopleHousingType HousingType)
    : ISaveHomeTypeSegmentCommand;
