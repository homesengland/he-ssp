using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveMoveOnAccommodationCommand(string ApplicationId, string HomeTypeId, YesNoType IntendedAsMoveOnAccommodation)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
