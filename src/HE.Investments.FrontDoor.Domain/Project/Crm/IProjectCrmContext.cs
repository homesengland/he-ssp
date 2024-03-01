using HE.Investments.Account.Shared.User;

namespace HE.Investments.FrontDoor.Domain.Project.Crm;

public interface IProjectCrmContext
{
    Task<IList<ProjectDto>> GetOrganisationProjects(Guid organisationId, CancellationToken cancellationToken);

    Task<IList<ProjectDto>> GetUserProjects(string userGlobalId, Guid organisationId, CancellationToken cancellationToken);

    Task<ProjectDto> GetOrganisationProjectById(string projectId, Guid organisationId, CancellationToken cancellationToken);

    Task<ProjectDto> GetUserProjectById(string projectId, string userGlobalId, Guid organisationId, CancellationToken cancellationToken);

    Task<string> Save(ProjectDto dto, UserAccount userAccount, CancellationToken cancellationToken);
}

public record ProjectDto(string id, string name);
