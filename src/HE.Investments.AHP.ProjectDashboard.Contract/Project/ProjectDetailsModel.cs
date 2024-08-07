using HE.Investments.Common.Contract.Pagination;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.AHP.ProjectDashboard.Contract.Project;

public record ProjectDetailsModel(
    FrontDoorProjectId ProjectId,
    string ProjectName,
    string ProgrammeName,
    string OrganisationName,
    PaginationResult<ApplicationProjectModel> Applications,
    PaginationResult<AllocationProjectModel> Allocations,
    bool IsReadOnly);
