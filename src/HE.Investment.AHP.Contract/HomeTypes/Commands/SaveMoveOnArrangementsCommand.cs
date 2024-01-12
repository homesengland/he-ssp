using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveMoveOnArrangementsCommand(
        AhpApplicationId ApplicationId,
        HomeTypeId HomeTypeId,
        string? MoveOnArrangements)
    : ISaveHomeTypeSegmentCommand;
