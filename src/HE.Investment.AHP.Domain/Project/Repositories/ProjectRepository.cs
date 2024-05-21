using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Project.Crm;
using HE.Investment.AHP.Domain.Project.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investment.AHP.Domain.Site.Mappers;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.FrontDoor.Shared.Project.Data;

namespace HE.Investment.AHP.Domain.Project.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IProjectCrmContext _projectCrmContext;

    private readonly SiteStatusMapper _siteStatusMapper = new();

    public ProjectRepository(IProjectCrmContext projectCrmContext)
    {
        _projectCrmContext = projectCrmContext;
    }

    public async Task<AhpProjectApplications> GetProject(AhpProjectId id, AhpUserAccount userAccount, CancellationToken cancellationToken)
    {
        var project = await _projectCrmContext.GetProject(
            id.ToString(),
            userAccount.UserGlobalId.ToString(),
            userAccount.SelectedOrganisationId().ToString(),
            userAccount.Consortium.ConsortiumId.ToString(),
            cancellationToken);

        var applications = project.Applications?
            .OrderByDescending(x => x.LastModificationDate)
            .Select(x => new AhpProjectApplication(
                AhpApplicationId.From(x.ApplicationId),
                new ApplicationName(x.ApplicationName),
                ApplicationStatusMapper.MapToPortalStatus(x.ApplicationStatus),
                new SchemeFunding((int?)x.RequiredFunding, x.NoOfHomes),
                ApplicationTenureMapper.ToDomain(x.Tenure)!.Value,
                x.LastModificationDate))
            .ToList();

        return new AhpProjectApplications(
            id,
            new AhpProjectName(project.ProjectName),
            applications);
    }

    public async Task<AhpProjectSites> GetProjectSites(AhpProjectId id, AhpUserAccount userAccount, CancellationToken cancellationToken)
    {
        var projectSites = await _projectCrmContext.GetProjectSites(
            id.ToString(),
            userAccount.UserGlobalId.ToString(),
            userAccount.SelectedOrganisationId().ToString(),
            userAccount.Consortium.ConsortiumId.ToString(),
            cancellationToken);

        return new AhpProjectSites(
            id,
            new AhpProjectName(projectSites.ProjectName),
            projectSites.Sites.Select(x => new AhpProjectSite(
                    SiteId.From(x.id),
                    new SiteName(x.name),
                    _siteStatusMapper.ToDomain(x.status)!.Value,
                    LocalAuthorityMapper.FromDto(x.localAuthority)))
                .ToList());
    }

    public async Task<PaginationResult<AhpProjectSites>> GetProjects(
        PaginationRequest paginationRequest,
        AhpUserAccount userAccount,
        CancellationToken cancellationToken)
    {
        var paging = new PagingRequestDto { pageNumber = paginationRequest.Page, pageSize = paginationRequest.ItemsPerPage };
        var projects = await _projectCrmContext.GetProjects(
            userAccount.UserGlobalId.ToString(),
            userAccount.SelectedOrganisationId().ToGuidAsString(),
            ShortGuid.ToGuidAsString(userAccount.Consortium.ConsortiumId.ToString()),
            paging,
            cancellationToken);

        return new PaginationResult<AhpProjectSites>(
            projects.items.Select(CreateAhpProjectEntity).ToList(),
            paginationRequest.Page,
            paginationRequest.ItemsPerPage,
            projects.totalItemsCount);
    }

    public async Task<AhpProjectId> CreateProject(ProjectPrefillData frontDoorProject, AhpUserAccount userAccount, CancellationToken cancellationToken)
    {
        var projectId = await _projectCrmContext.CreateProject(
            userAccount.UserGlobalId.ToString(),
            userAccount.SelectedOrganisationId().ToGuidAsString(),
            userAccount.Consortium.ConsortiumId.ToString(),
            frontDoorProject.Id.ToGuidAsString(),
            frontDoorProject.Name,
            CreateProjectSitesDto(frontDoorProject.Sites),
            cancellationToken);

        return AhpProjectId.From(projectId);
    }

    private AhpProjectSites CreateAhpProjectEntity(ProjectDto projectDto)
    {
        var sites = projectDto.Sites?
            .Select(s => new AhpProjectSite(
                SiteId.From(s.id),
                new SiteName(s.name),
                new SiteStatusMapper().ToDomain(s.status)!.Value,
                null))
            .ToList();

        return new AhpProjectSites(
            AhpProjectId.From(projectDto.ProjectId),
            new AhpProjectName(projectDto.ProjectName),
            sites);
    }

    private List<SiteDto> CreateProjectSitesDto(IList<SitePrefillData>? sites)
    {
        return sites?.Select(x => new SiteDto
        {
            id = x.Id.ToString(),
            name = x.Name.ToString(),
        }).ToList() ?? [];
    }
}
