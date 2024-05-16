using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Domain.Project.Entities;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.Common.Contract.Pagination;

namespace HE.Investment.AHP.Domain.Project.Repositories;

public interface IProjectRepository
{
    Task<AhpProjectEntity> GetProject(AhpProjectId id, AhpUserAccount userAccount, CancellationToken cancellationToken);

    Task<PaginationResult<AhpProjectEntity>> GetProjects(PaginationRequest paginationRequest, AhpUserAccount userAccount, CancellationToken cancellationToken);
}
