using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveBuildingInformationCommand(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId, BuildingType BuildingType)
    : ISaveHomeTypeSegmentCommand;
