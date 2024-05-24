using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Contract.Project.Events;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Project.Crm;
using HE.Investment.AHP.Domain.Project.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investment.AHP.Domain.Site.Mappers;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Data;

namespace HE.Investment.AHP.Domain.Project.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IProjectCrmContext _projectCrmContext;

    private readonly SiteStatusMapper _siteStatusMapper = new();

    private readonly IEventDispatcher _eventDispatcher;

    public ProjectRepository(IProjectCrmContext projectCrmContext, IEventDispatcher eventDispatcher)
    {
        _projectCrmContext = projectCrmContext;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<AhpProjectApplications> GetProjectApplications(FrontDoorProjectId id, AhpUserAccount userAccount, CancellationToken cancellationToken)
    {
        var project = await _projectCrmContext.GetProject(
            id.ToString(),
            userAccount.UserGlobalId.ToString(),
            userAccount.SelectedOrganisationId().ToString(),
            userAccount.Consortium.GetConsortiumIdAsString(),
            cancellationToken);

        var applications = project.ListOfApplications?
            .OrderByDescending(x => x.lastExternalModificationOn)
            .Select(x => new AhpProjectApplication(
                AhpApplicationId.From(x.id),
                new ApplicationName(x.name),
                ApplicationStatusMapper.MapToPortalStatus(x.applicationStatus),
                new SchemeFunding((int?)x.fundingRequested, x.noOfHomes),
                ApplicationTenureMapper.ToDomain(x.tenure)!.Value,
                x.lastExternalModificationOn))
            .ToList();

        return new AhpProjectApplications(
            id,
            new AhpProjectName(project.AhpProjectName),
            applications);
    }

    public async Task<AhpProjectSites> GetProjectSites(FrontDoorProjectId id, AhpUserAccount userAccount, CancellationToken cancellationToken)
    {
        var projectSites = await _projectCrmContext.GetProject(
            id.ToString(),
            userAccount.UserGlobalId.ToString(),
            userAccount.SelectedOrganisationId().ToString(),
            userAccount.Consortium.GetConsortiumIdAsString(),
            cancellationToken);

        return new AhpProjectSites(
            id,
            new AhpProjectName(projectSites.AhpProjectName),
            projectSites.ListOfSites.Select(x => new AhpProjectSite(
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
            userAccount.Consortium.GetConsortiumIdAsString(),
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
            userAccount.Consortium.GetConsortiumIdAsString(),
            frontDoorProject.Id.ToGuidAsString(),
            frontDoorProject.Name,
            CreateProjectSitesDto(frontDoorProject.Sites),
            cancellationToken);

        await _eventDispatcher.Publish(new AhpProjectHasBeenCreatedEvent(frontDoorProject.Id), cancellationToken);

        return AhpProjectId.From(projectId);
    }

    private AhpProjectSites CreateAhpProjectEntity(AhpProjectDto ahpProjectDto)
    {
        var sites = ahpProjectDto.ListOfSites?
            .Select(s => new AhpProjectSite(
                SiteId.From(s.id),
                new SiteName(s.name),
                new SiteStatusMapper().ToDomain(s.status)!.Value,
                null))
            .ToList();

        return new AhpProjectSites(
            FrontDoorProjectId.From(ahpProjectDto.FrontDoorProjectId),
            new AhpProjectName(ahpProjectDto.AhpProjectName),
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
