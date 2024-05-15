using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Domain.Project.Entities;
using HE.Investment.AHP.Domain.UserContext;

namespace HE.Investment.AHP.Domain.Project.Repositories;

public interface IProjectRepository
{
    public Task<AhpProjectEntity> GetProject(AhpProjectId id, AhpUserAccount userAccount, CancellationToken cancellationToken);
}
