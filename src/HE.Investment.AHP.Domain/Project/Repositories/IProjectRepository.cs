using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Domain.Project.ValueObjects;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.Common.Contract.Pagination;

namespace HE.Investment.AHP.Domain.Project.Repositories;

public interface IProjectRepository
{
    Task<AhpProjectApplications> GetProject(AhpProjectId id, AhpUserAccount userAccount, CancellationToken cancellationToken);

    Task<PaginationResult<AhpProjectSites>> GetProjects(PaginationRequest paginationRequest, AhpUserAccount userAccount, CancellationToken cancellationToken);

    Task<AhpProjectSites> GetProjectSites(AhpProjectId id, AhpUserAccount userAccount, CancellationToken cancellationToken);
}
