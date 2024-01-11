using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveExitPlanCommand(
        AhpApplicationId ApplicationId,
        string HomeTypeId,
        string? ExitPlan)
    : ISaveHomeTypeSegmentCommand;
