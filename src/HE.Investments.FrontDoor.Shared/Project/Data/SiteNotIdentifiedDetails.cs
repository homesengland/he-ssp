using HE.Investments.FrontDoor.Shared.Project.Contract;

namespace HE.Investments.FrontDoor.Shared.Project.Data;

public record SiteNotIdentifiedDetails(ProjectGeographicFocus GeographicFocus, IReadOnlyCollection<RegionType>? Regions, int NumberOfHomes);
