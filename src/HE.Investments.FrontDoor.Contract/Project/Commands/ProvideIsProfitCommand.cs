using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideIsProfitCommand(FrontDoorProjectId ProjectId, bool? IsProfit) : IProvideProjectDetailsCommand;
