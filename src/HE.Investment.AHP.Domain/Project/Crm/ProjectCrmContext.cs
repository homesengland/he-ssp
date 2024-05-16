using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Application.Crm;
using HE.Investment.AHP.Domain.Site.Crm;

namespace HE.Investment.AHP.Domain.Project.Crm;

public class ProjectCrmContext : IProjectCrmContext
{
    private readonly ProjectDto _mockedProjectDto =
        new()
        {
            ProjectId = "885f479a-eed7-4745-817d-9c7968db4852",
            ProjectName = "Mocked project",
            Sites =
            {
                new SiteDto { id = Guid.NewGuid().ToString(), name = "first new site", status = 858110001, },
                new SiteDto { id = Guid.NewGuid().ToString(), name = "second new site", status = 858110000, },
                new SiteDto { id = Guid.NewGuid().ToString(), name = "third new site", status = 858110001, },
            },
        };

    private readonly IApplicationCrmContext _applicationCrmContext;

    private readonly ISiteCrmContext _siteCrmContext;

    public ProjectCrmContext(IApplicationCrmContext applicationCrmContext, ISiteCrmContext siteCrmContext)
    {
        _applicationCrmContext = applicationCrmContext;
        _siteCrmContext = siteCrmContext;
    }

    public async Task<ProjectDto> GetProject(
        string projectId,
        string userId,
        string organisationId,
        string? consortiumId,
        CancellationToken cancellationToken)
    {
        var applications = await _applicationCrmContext.GetUserApplications(organisationId, cancellationToken);
        _mockedProjectDto.Applications = applications
            .Where(x => x.tenure.HasValue)
            .Select(x =>
                new ProjectApplicationDto
                {
                    ApplicationId = x.id,
                    ApplicationStatus = x.applicationStatus,
                    ApplicationName = x.name,
                    LastModificationDate = x.lastExternalModificationOn,
                    RequiredFunding = x.fundingRequested,
                    NoOfHomes = x.noOfHomes,
                    Tenure = x.tenure,
                })
            .ToList();

        return _mockedProjectDto;
    }

    public Task<PagedResponseDto<ProjectDto>> GetProjects(
        string userId,
        string organisationId,
        string? consortiumId,
        PagingRequestDto pagination,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new PagedResponseDto<ProjectDto>
        {
            items = { _mockedProjectDto },
            totalItemsCount = 1,
            paging = new PagingRequestDto { pageNumber = 1, pageSize = 1, },
        });
    }

    public async Task<ProjectSitesDto> GetProjectSites(
        string projectId,
        string userId,
        string organisationId,
        string? consortiumId,
        CancellationToken cancellationToken)
    {
        var sites = await _siteCrmContext.GetUserSites(userId, new PagingRequestDto { pageNumber = 1, pageSize = 100 }, cancellationToken);
        return new ProjectSitesDto
        {
            ProjectId = _mockedProjectDto.ProjectId,
            ProjectName = _mockedProjectDto.ProjectName,
            Sites = sites.items,
        };
    }
}
