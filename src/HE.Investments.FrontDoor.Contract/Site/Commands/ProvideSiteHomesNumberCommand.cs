using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Contract.Site.Commands;

public record ProvideSiteHomesNumberCommand(FrontDoorProjectId ProjectId, FrontDoorSiteId SiteId, string? HouseNumber) : IProvideSiteDetailsCommand;
