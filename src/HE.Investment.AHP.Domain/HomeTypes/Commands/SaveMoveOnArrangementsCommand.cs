namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveMoveOnArrangementsCommand(
        string ApplicationId,
        string HomeTypeId,
        string? MoveOnArrangements)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
