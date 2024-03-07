namespace HE.Investments.FrontDoor.Contract.Site.Commands;

public record ProvideSiteHomesNumberCommand(FrontDoorSiteId SiteId, string? HouseNumber) : IProvideSiteDetailsCommand;
