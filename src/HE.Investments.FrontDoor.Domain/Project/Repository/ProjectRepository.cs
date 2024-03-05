using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Domain.Project.Crm;
using HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project.Repository;

public class ProjectRepository : IProjectRepository
{
    private readonly IProjectCrmContext _crmContext;

    private readonly AffordableHomesAmountMapper _affordableHomesAmountMapper = new();

    public ProjectRepository(IProjectCrmContext crmContext)
    {
        _crmContext = crmContext;
    }

    public async Task<IList<ProjectEntity>> GetProjects(UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var projects = userAccount.CanViewAllApplications()
            ? await _crmContext.GetOrganisationProjects(userAccount.UserGlobalId.Value, organisationId, cancellationToken)
            : await _crmContext.GetUserProjects(userAccount.UserGlobalId.Value, organisationId, cancellationToken);

        return projects.Select(MapToEntity).ToList();
    }

    public async Task<ProjectEntity> GetProject(FrontDoorProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var project = userAccount.CanViewAllApplications()
            ? await _crmContext.GetOrganisationProjectById(projectId.Value, userAccount.UserGlobalId.Value, organisationId, cancellationToken)
            : await _crmContext.GetUserProjectById(projectId.Value, userAccount.UserGlobalId.Value, organisationId, cancellationToken);

        return MapToEntity(project);
    }

    public async Task<ProjectEntity> Save(ProjectEntity project, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var dto = new FrontDoorProjectDto
        {
            ProjectId = project.Id.IsNew ? null : project.Id.Value,
            ProjectName = project.Name.Value,
            OrganisationId = userAccount.SelectedOrganisationId().Value,
            externalId = userAccount.UserGlobalId.Value,
            AmountofAffordableHomes = _affordableHomesAmountMapper.ToDto(project.AffordableHomesAmount.AffordableHomesAmount),
        };

        var projectId = await _crmContext.Save(dto, userAccount, cancellationToken);
        if (project.Id.IsNew)
        {
            project.SetId(new FrontDoorProjectId(projectId));
        }

        return project;
    }

    public Task<bool> DoesExist(ProjectName name, FrontDoorProjectId? exceptProjectId, CancellationToken cancellationToken)
    {
        // TODO: AB#91792 Validate project name uniqueness
        return Task.FromResult(false);
    }

    private ProjectEntity MapToEntity(FrontDoorProjectDto dto)
    {
        return new ProjectEntity(
            new FrontDoorProjectId(dto.ProjectId),
            new ProjectName(dto.ProjectName),
            ProjectAffordableHomesAmount.Create(_affordableHomesAmountMapper.ToDomain(dto.AmountofAffordableHomes)));
    }
}
