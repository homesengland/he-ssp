using HE.Investments.FrontDoor.Contract.Project.Enums;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideRegionCommand(FrontDoorProjectId ProjectId, IList<RegionType> Regions) : IProvideProjectDetailsCommand;
