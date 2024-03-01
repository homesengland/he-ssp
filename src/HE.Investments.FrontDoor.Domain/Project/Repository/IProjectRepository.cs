using HE.Investments.Account.Shared.User;

namespace HE.Investments.FrontDoor.Domain.Project.Repository;

public interface IProjectRepository
{
    Task<IList<ProjectEntity>> GetProjects(UserAccount userAccount, CancellationToken cancellationToken);

    Task<ProjectEntity> GetProject(string projectId, UserAccount userAccount, CancellationToken cancellationToken);
}
