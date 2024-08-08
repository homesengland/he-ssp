using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.AHP.ProjectDashboard.Contract.Project;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.ValueObjects;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Data;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Project.Repositories;

public interface IProjectRepository
{
    Task<AhpProjectOverview> GetProjectOverview(FrontDoorProjectId id, ConsortiumUserAccount userAccount, CancellationToken cancellationToken);

    Task<PaginationResult<AhpProjectSites>> GetProjects(PaginationRequest paginationRequest, ConsortiumUserAccount userAccount, CancellationToken cancellationToken);

    Task<AhpProjectSites> GetProjectSites(FrontDoorProjectId id, ConsortiumUserAccount userAccount, CancellationToken cancellationToken);

    Task<AhpProjectId> CreateProject(ProjectPrefillData frontDoorProject, ConsortiumUserAccount userAccount, CancellationToken cancellationToken);

    Task<AhpProjectDto?> TryGetProject(FrontDoorProjectId id, ConsortiumUserAccount userAccount, CancellationToken cancellationToken);
}
