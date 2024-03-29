using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideIsFundingRequiredCommand(FrontDoorProjectId ProjectId, bool? IsFundingRequired) : IProvideProjectDetailsCommand;
