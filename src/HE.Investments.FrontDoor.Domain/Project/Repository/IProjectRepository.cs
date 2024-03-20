using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Domain;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Domain.Project.Repository;

public interface IProjectRepository
{
    Task<IList<ProjectEntity>> GetProjects(UserAccount userAccount, CancellationToken cancellationToken);

    Task<ProjectEntity> GetProject(FrontDoorProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<ProjectEntity> Save(ProjectEntity project, UserAccount userAccount, CancellationToken cancellationToken);

    Task<bool> DoesExist(ProjectName name, UserAccount userAccount, CancellationToken cancellationToken);

    Task DispatchEvents(DomainEntity domainEntity, CancellationToken cancellationToken);
}
