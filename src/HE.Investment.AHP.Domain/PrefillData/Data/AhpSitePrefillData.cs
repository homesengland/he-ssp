using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investment.AHP.Domain.PrefillData.Data;

public record AhpSitePrefillData(
    FrontDoorProjectId ProjectId,
    FrontDoorSiteId SiteId,
    string? SiteName,
    string? LocalAuthorityName,
    SitePlanningStatus PlanningStatus);
