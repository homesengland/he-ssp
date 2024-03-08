namespace HE.Investments.FrontDoor.Contract.Site.Commands;

public record ProvideHomesNumberCommand(FrontDoorSiteId SiteId, string? HouseNumber) : IProvideSiteDetailsCommand;
