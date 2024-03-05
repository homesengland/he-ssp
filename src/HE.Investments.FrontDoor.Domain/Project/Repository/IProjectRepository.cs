using HE.Investments.Account.Shared.User;
using HE.Investments.FrontDoor.Contract.Project;

namespace HE.Investments.FrontDoor.Domain.Project.Repository;

public interface IProjectRepository
{
    Task<IList<ProjectEntity>> GetProjects(UserAccount userAccount, CancellationToken cancellationToken);

    Task<ProjectEntity> GetProject(FrontDoorProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<ProjectEntity> Save(ProjectEntity project, UserAccount userAccount, CancellationToken cancellationToken);
}
