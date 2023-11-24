namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveExitPlanCommand(
        string ApplicationId,
        string HomeTypeId,
        string? ExitPlan)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
