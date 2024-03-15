using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideHomesNumberCommand(FrontDoorProjectId ProjectId, string? HomesNumber) : IProvideProjectDetailsCommand;
