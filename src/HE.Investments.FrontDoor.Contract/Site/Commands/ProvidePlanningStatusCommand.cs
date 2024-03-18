using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Contract.Site.Commands;

public record ProvidePlanningStatusCommand(FrontDoorProjectId ProjectId, FrontDoorSiteId SiteId, SitePlanningStatus? PlanningStatus) : IProvideSiteDetailsCommand;
