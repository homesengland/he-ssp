using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveExitPlanCommand(
        AhpApplicationId ApplicationId,
        HomeTypeId HomeTypeId,
        string? ExitPlan)
    : ISaveHomeTypeSegmentCommand;
