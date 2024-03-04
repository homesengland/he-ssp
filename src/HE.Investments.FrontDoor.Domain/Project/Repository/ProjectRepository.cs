using HE.Investments.Account.Shared.User;
using HE.Investments.FrontDoor.Domain.Project.Crm;

namespace HE.Investments.FrontDoor.Domain.Project.Repository;

public class ProjectRepository : IProjectRepository
{
    private readonly IProjectCrmContext _crmContext;

    public ProjectRepository(IProjectCrmContext crmContext)
    {
        _crmContext = crmContext;
    }

    public async Task<IList<ProjectEntity>> GetProjects(UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var projects = userAccount.CanViewAllApplications()
            ? await _crmContext.GetOrganisationProjects(organisationId, cancellationToken)
            : await _crmContext.GetUserProjects(userAccount.UserGlobalId.Value, organisationId, cancellationToken);

        return projects.Select(x => new ProjectEntity(x.id, x.name)).ToList();
    }

    public async Task<ProjectEntity> GetProject(string projectId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var project = userAccount.CanViewAllApplications()
            ? await _crmContext.GetOrganisationProjectById(projectId, organisationId, cancellationToken)
            : await _crmContext.GetUserProjectById(projectId, userAccount.UserGlobalId.Value, organisationId, cancellationToken);

        return new ProjectEntity(project.id, project.name);
    }
}
