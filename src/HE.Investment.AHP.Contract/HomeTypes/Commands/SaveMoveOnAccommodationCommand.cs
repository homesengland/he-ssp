using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveMoveOnAccommodationCommand(AhpApplicationId ApplicationId, string HomeTypeId, YesNoType IntendedAsMoveOnAccommodation)
    : ISaveHomeTypeSegmentCommand;
