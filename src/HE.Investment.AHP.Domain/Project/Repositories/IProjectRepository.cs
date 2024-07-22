using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Domain.Project.ValueObjects;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Data;

namespace HE.Investment.AHP.Domain.Project.Repositories;

public interface IProjectRepository
{
    Task<AhpProjectOverview> GetProjectOverview(FrontDoorProjectId id, ConsortiumUserAccount userAccount, CancellationToken cancellationToken);

    Task<PaginationResult<AhpProjectSites>> GetProjects(PaginationRequest paginationRequest, ConsortiumUserAccount userAccount, CancellationToken cancellationToken);

    Task<AhpProjectSites> GetProjectSites(FrontDoorProjectId id, ConsortiumUserAccount userAccount, CancellationToken cancellationToken);

    Task<AhpProjectId> CreateProject(ProjectPrefillData frontDoorProject, ConsortiumUserAccount userAccount, CancellationToken cancellationToken);

    Task<AhpProjectDto?> TryGetProject(FrontDoorProjectId id, ConsortiumUserAccount userAccount, CancellationToken cancellationToken);
}
