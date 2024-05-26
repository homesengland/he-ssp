using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Domain.Project.ValueObjects;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Data;

namespace HE.Investment.AHP.Domain.Project.Repositories;

public interface IProjectRepository
{
    Task<AhpProjectApplications> GetProjectApplications(FrontDoorProjectId id, AhpUserAccount userAccount, CancellationToken cancellationToken);

    Task<PaginationResult<AhpProjectSites>> GetProjects(PaginationRequest paginationRequest, AhpUserAccount userAccount, CancellationToken cancellationToken);

    Task<AhpProjectSites> GetProjectSites(FrontDoorProjectId id, AhpUserAccount userAccount, CancellationToken cancellationToken);

    Task<AhpProjectId> CreateProject(ProjectPrefillData frontDoorProject, AhpUserAccount userAccount, CancellationToken cancellationToken);
}
