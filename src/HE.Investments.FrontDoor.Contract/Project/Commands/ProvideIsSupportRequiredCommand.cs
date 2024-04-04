using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideIsSupportRequiredCommand(FrontDoorProjectId ProjectId, bool? IsSupportRequired) : IProvideProjectDetailsCommand;
