using HE.Investments.FrontDoor.Contract.Site.Enums;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Contract.Site.Commands;

public record RemoveSiteCommand(FrontDoorProjectId ProjectId, FrontDoorSiteId SiteId, RemoveSiteAnswer? RemoveSiteAnswer) : IProvideSiteDetailsCommand;
