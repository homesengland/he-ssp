using HE.Investments.FrontDoor.Contract.Project.Enums;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideGeographicFocusCommand(FrontDoorProjectId ProjectId, ProjectGeographicFocus GeographicFocus)
    : IProvideProjectDetailsCommand;
