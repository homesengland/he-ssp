using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Contract.Site.Commands;

public record ProvideSiteNameCommand(FrontDoorProjectId ProjectId, FrontDoorSiteId SiteId, string? Name) : IProvideSiteDetailsCommand;
