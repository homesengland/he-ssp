using HE.Investments.Common.Contract.Enum;

namespace HE.Investments.FrontDoor.Shared.Project.Data;

public record SitePrefillData(FrontDoorSiteId Id, string Name, int? NumberOfHomes, SitePlanningStatus PlanningStatus, string? LocalAuthorityName);
