using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Project.Crm;
using HE.Investment.AHP.Domain.Project.Entities;
using HE.Investment.AHP.Domain.Project.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investment.AHP.Domain.Site.Mappers;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.Common.CRM.Mappers;

namespace HE.Investment.AHP.Domain.Project.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IProjectCrmContext _projectCrmContext;

    private readonly SiteStatusMapper _siteStatusMapper = new();

    public ProjectRepository(IProjectCrmContext projectCrmContext)
    {
        _projectCrmContext = projectCrmContext;
    }

    public async Task<AhpProjectEntity> GetProject(AhpProjectId id, AhpUserAccount userAccount, CancellationToken cancellationToken)
    {
        var project = await _projectCrmContext.GetProject(
            id.ToString(),
            userAccount.UserGlobalId.ToString(),
            userAccount.SelectedOrganisationId().ToString(),
            userAccount.Consortium.ConsortiumId.ToString(),
            cancellationToken);

        return new AhpProjectEntity(
            id,
            new AhpProjectName(project.ProjectName),
            project.Applications.Select(x => new AhpProjectApplication(
                    AhpApplicationId.From(x.ApplicationId),
                    new ApplicationName(x.ApplicationName),
                    ApplicationStatusMapper.MapToPortalStatus(x.ApplicationStatus),
                    new SchemeFunding((int?)x.RequiredFunding, x.NoOfHomes),
                    ApplicationTenureMapper.ToDomain(x.Tenure)!.Value))
                .ToList());
    }

    public async Task<AhpProjectSitesEntity> GetProjectSites(AhpProjectId id, AhpUserAccount userAccount, CancellationToken cancellationToken)
    {
        var projectSites = await _projectCrmContext.GetProjectSites(
            id.ToString(),
            userAccount.UserGlobalId.ToString(),
            userAccount.SelectedOrganisationId().ToString(),
            userAccount.Consortium.ConsortiumId.ToString(),
            cancellationToken);

        return new AhpProjectSitesEntity(
            id,
            new AhpProjectName(projectSites.ProjectName),
            projectSites.Sites.Select(x => new ProjectSite(
                    SiteId.From(x.id),
                    new SiteName(x.name),
                    _siteStatusMapper.ToDomain(x.status)!.Value,
                    LocalAuthorityMapper.FromDto(x.localAuthority)))
                .ToList());
    }
}
