using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Loans.BusinessLogic.Projects.Enums;

namespace HE.Investments.Loans.BusinessLogic.PrefillData.Data;

public record LoanProjectPrefillData(
    FrontDoorProjectId ProjectId,
    FrontDoorSiteId SiteId,
    string Name,
    int? NumberOfHomes,
    PlanningPermissionStatus? PlanningPermissionStatus,
    string? LocalAuthorityName);
