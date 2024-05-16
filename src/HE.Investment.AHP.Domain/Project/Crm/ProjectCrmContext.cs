using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Application.Crm;

namespace HE.Investment.AHP.Domain.Project.Crm;

public class ProjectCrmContext : IProjectCrmContext
{
    private readonly ProjectDto _mockedProjectDto =
        new()
        {
            ProjectId = "885f479a-eed7-4745-817d-9c7968db4852",
            ProjectName = "Mocked project",
            Sites = new List<SiteDto>
            {
                new() { id = Guid.NewGuid().ToString(), name = "first new site", status = 858110001, },
                new() { id = Guid.NewGuid().ToString(), name = "second new site", status = 858110000, },
                new() { id = Guid.NewGuid().ToString(), name = "third new site", status = 858110001, },
            },
        };

    private readonly IApplicationCrmContext _applicationCrmContext;

    public ProjectCrmContext(IApplicationCrmContext applicationCrmContext)
    {
        _applicationCrmContext = applicationCrmContext;
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
            items = new List<ProjectDto> { _mockedProjectDto },
            totalItemsCount = 1,
            paging = new PagingRequestDto { pageNumber = 1, pageSize = 1, },
        });
    }
}
