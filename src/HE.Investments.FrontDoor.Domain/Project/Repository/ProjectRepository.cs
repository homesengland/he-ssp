using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.FrontDoor.Domain.Project.Crm;
using HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Domain.Project.Repository;

public class ProjectRepository : IProjectRepository
{
    private readonly IProjectCrmContext _crmContext;

    private readonly IProjectCrmMapper _crmMapper;

    private readonly IEventDispatcher _eventDispatcher;

    public ProjectRepository(IProjectCrmContext crmContext, IProjectCrmMapper crmMapper, IEventDispatcher eventDispatcher)
    {
        _crmContext = crmContext;
        _crmMapper = crmMapper;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<IList<ProjectEntity>> GetProjects(UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var projects = userAccount.CanViewAllApplications()
            ? await _crmContext.GetOrganisationProjects(userAccount.UserGlobalId.Value, organisationId, cancellationToken)
            : await _crmContext.GetUserProjects(userAccount.UserGlobalId.Value, organisationId, cancellationToken);

        return projects.OrderByDescending(x => x.CreatedOn).Select(_crmMapper.ToEntity).ToList();
    }

    public async Task<ProjectEntity> GetProject(FrontDoorProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var project = userAccount.CanViewAllApplications()
            ? await _crmContext.GetOrganisationProjectById(projectId.Value, userAccount.UserGlobalId.Value, organisationId, cancellationToken)
            : await _crmContext.GetUserProjectById(projectId.Value, userAccount.UserGlobalId.Value, organisationId, cancellationToken);

        return _crmMapper.ToEntity(project);
    }

    public async Task<ProjectEntity> Save(ProjectEntity project, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (!project.IsModified)
        {
            return project;
        }

        var dto = _crmMapper.ToDto(project, userAccount);
        var projectId = await _crmContext.Save(dto, userAccount, cancellationToken);
        if (project.Id.IsNew)
        {
            project.New(new FrontDoorProjectId(projectId));
        }

        await DispatchEvents(project, cancellationToken);

        return project;
    }

    public async Task<bool> DoesExist(ProjectName name, UserAccount userAccount, CancellationToken cancellationToken)
    {
        return await _crmContext.IsThereProjectWithName(name.Value, userAccount.SelectedOrganisationId().Value, cancellationToken);
    }

    private async Task DispatchEvents(DomainEntity domainEntity, CancellationToken cancellationToken)
    {
        await _eventDispatcher.Publish(domainEntity, cancellationToken);
    }
}
