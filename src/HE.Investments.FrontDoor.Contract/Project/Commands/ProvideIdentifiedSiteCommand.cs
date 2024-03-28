using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideIdentifiedSiteCommand(FrontDoorProjectId ProjectId, bool? IsSiteIdentified) : IProvideProjectDetailsCommand;
