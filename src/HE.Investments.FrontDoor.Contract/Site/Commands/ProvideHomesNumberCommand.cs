using HE.Investments.FrontDoor.Contract.Project;

namespace HE.Investments.FrontDoor.Contract.Site.Commands;

public record ProvideHomesNumberCommand(FrontDoorProjectId ProjectId, FrontDoorSiteId SiteId, string? HouseNumber) : IProvideSiteDetailsCommand;
