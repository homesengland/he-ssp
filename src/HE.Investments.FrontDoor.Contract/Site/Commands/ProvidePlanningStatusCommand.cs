using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Contract.Project;

namespace HE.Investments.FrontDoor.Contract.Site.Commands;

public record ProvidePlanningStatusCommand(FrontDoorProjectId ProjectId, FrontDoorSiteId SiteId, SitePlanningStatus? PlanningStatus) : IProvideSiteDetailsCommand;
