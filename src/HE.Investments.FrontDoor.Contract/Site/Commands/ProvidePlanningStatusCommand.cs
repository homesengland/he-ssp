using HE.Investments.Common.Contract.Enum;

namespace HE.Investments.FrontDoor.Contract.Site.Commands;

public record ProvidePlanningStatusCommand(FrontDoorSiteId SiteId, SitePlanningStatus? PlanningStatus) : IProvideSiteDetailsCommand;
