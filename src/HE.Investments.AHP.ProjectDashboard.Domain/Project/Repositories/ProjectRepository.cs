using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Site;
using HE.Investments.AHP.ProjectDashboard.Contract.Project;
using HE.Investments.AHP.ProjectDashboard.Contract.Project.Events;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.Crm;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.Mappers;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Data;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Project.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IProjectCrmContext _projectCrmContext;

    private readonly EnumMapper<Tenure> _tenureMapper = new TenureMapper();

    private readonly EnumMapper<SiteStatus> _siteStatusMapper = new SiteStatusMapper();

    private readonly IEventDispatcher _eventDispatcher;

    public ProjectRepository(IProjectCrmContext projectCrmContext, IEventDispatcher eventDispatcher)
    {
        _projectCrmContext = projectCrmContext;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<AhpProjectDto?> TryGetProject(FrontDoorProjectId id, ConsortiumUserAccount userAccount, CancellationToken cancellationToken)
    {
        try
        {
            var project = await _projectCrmContext.GetProject(
                id.ToGuidAsString(),
                userAccount.UserGlobalId.ToString(),
                userAccount.SelectedOrganisationId().ToString(),
                userAccount.Consortium.GetConsortiumIdAsString(),
                cancellationToken);
            return project;
        }
        catch (NotFoundException)
        {
            return null;
        }
    }

    public async Task<AhpProjectOverview> GetProjectOverview(FrontDoorProjectId id, ConsortiumUserAccount userAccount, CancellationToken cancellationToken)
    {
        var project = await _projectCrmContext.GetProject(
            id.ToGuidAsString(),
            userAccount.UserGlobalId.ToString(),
            userAccount.SelectedOrganisationId().ToString(),
            userAccount.Consortium.GetConsortiumIdAsString(),
            cancellationToken);

        return new AhpProjectOverview(
            id,
            new AhpProjectName(project.AhpProjectName),
            MapAhpApplications(project),
            MapAhpAllocation(project));
    }

    public async Task<AhpProjectSites> GetProjectSites(FrontDoorProjectId id, ConsortiumUserAccount userAccount, CancellationToken cancellationToken)
    {
        var projectSites = await _projectCrmContext.GetProject(
            id.ToGuidAsString(),
            userAccount.UserGlobalId.ToString(),
            userAccount.SelectedOrganisationId().ToString(),
            userAccount.Consortium.GetConsortiumIdAsString(),
            cancellationToken);

        return new AhpProjectSites(
            id,
            new AhpProjectName(projectSites.AhpProjectName),
            projectSites.ListOfSites.Select(x => new AhpProjectSite(
                    x.fdSiteid.IsProvided() ? FrontDoorSiteId.From(x.fdSiteid) : null,
                    SiteId.From(x.id),
                    x.name,
                    _siteStatusMapper.ToDomain(x.status)!.Value,
                    MapLocalAuthority(x.localAuthority)))
                .ToList());
    }

    public async Task<PaginationResult<AhpProjectSites>> GetProjects(
        PaginationRequest paginationRequest,
        ConsortiumUserAccount userAccount,
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

    public async Task<AhpProjectId> CreateProject(ProjectPrefillData frontDoorProject, ConsortiumUserAccount userAccount, CancellationToken cancellationToken)
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

    private static LocalAuthority? MapLocalAuthority(SiteLocalAuthority? localAuthority)
    {
        return localAuthority is null || string.IsNullOrWhiteSpace(localAuthority.id) || string.IsNullOrWhiteSpace(localAuthority.name)
            ? null
            : new LocalAuthority(new LocalAuthorityCode(localAuthority.id), localAuthority.name);
    }

    private static List<SiteDto> CreateProjectSitesDto(IList<SitePrefillData>? sites)
    {
        return sites?.Select(x => new SiteDto
        {
            fdSiteid = x.Id.ToGuidAsString(),
            name = x.Name.ToString(),
        }).ToList() ?? [];
    }

    private static string? GetSiteLocalAuthority(string siteId, List<SiteDto> sites)
    {
        var site = sites.Find(x => x.id == siteId);
        return site?.localAuthority.name;
    }

    private AhpProjectSites CreateAhpProjectEntity(AhpProjectDto ahpProjectDto)
    {
        var sites = ahpProjectDto.ListOfSites?
            .Select(s => new AhpProjectSite(
                s.fdSiteid.IsProvided() ? FrontDoorSiteId.From(s.fdSiteid) : null,
                SiteId.From(s.id),
                s.name,
                _siteStatusMapper.ToDomain(s.status)!.Value,
                null))
            .ToList();

        return new AhpProjectSites(
            FrontDoorProjectId.From(ahpProjectDto.FrontDoorProjectId),
            new AhpProjectName(ahpProjectDto.AhpProjectName),
            sites);
    }

    private List<AhpProjectApplication>? MapAhpApplications(AhpProjectDto project)
    {
        return project.ListOfApplications?
            .OrderByDescending(x => x.lastExternalModificationOn)
            .Select(x => new AhpProjectApplication(
                AhpApplicationId.From(x.id),
                x.name,
                AhpApplicationStatusMapper.MapToPortalStatus(x.applicationStatus),
                (int)(x.fundingRequested ?? 0),
                x.noOfHomes ?? 0,
                _tenureMapper.ToDomain(x.tenure)!.Value,
                x.lastExternalModificationOn,
                GetSiteLocalAuthority(x.siteId, project.ListOfSites)))
            .ToList();
    }

    private List<AhpProjectAllocation>? MapAhpAllocation(AhpProjectDto project)
    {
        return project.ListOfAhpAllocations?
            .Select(x => new AhpProjectAllocation(
                x.Id,
                x.Name,
                x.Homes,
                _tenureMapper.ToDomain(x.Tenure)!.Value,
                x.LocalAuthority.name,
                null))
            .ToList();
    }
}
