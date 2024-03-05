namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideProjectNameCommand(FrontDoorProjectId ProjectId, string? Name) : IProvideProjectDetailsCommand;
