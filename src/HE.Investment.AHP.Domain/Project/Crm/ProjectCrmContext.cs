using System.Text.Json;
using System.Text.Json.Serialization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Domain.Application.Crm;
using HE.Investment.AHP.Domain.Site.Crm;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Project.Crm;

public class ProjectCrmContext : IProjectCrmContext
{
    private readonly List<ProjectDto> _mockedProjectDtoList =
    [
        new ProjectDto { ProjectName = LegacyProject.ProjectName, ProjectId = LegacyProject.ProjectId },
        new ProjectDto
        {
            ProjectName = "Second project",
            ProjectId = Guid.NewGuid().ToString(),
            Sites =
            [
                new SiteDto { id = Guid.NewGuid().ToString(), name = "first new site", status = 858110001, },
                new SiteDto { id = Guid.NewGuid().ToString(), name = "second new site", status = 858110000, },
                new SiteDto { id = Guid.NewGuid().ToString(), name = "third new site", status = 858110001, },
            ],
        },
        new ProjectDto
        {
            ProjectName = "Third project",
            ProjectId = Guid.NewGuid().ToString(),
            Sites =
            [
                new SiteDto { id = Guid.NewGuid().ToString(), name = "first new site", status = 858110001, },
                new SiteDto { id = Guid.NewGuid().ToString(), name = "second new site", status = 858110000, },
            ],
        },
        new ProjectDto { ProjectName = "Fourth project", ProjectId = Guid.NewGuid().ToString() },
    ];

    private readonly ISiteCrmContext _siteCrmContext;

    private readonly ICrmService _service;

    private readonly JsonSerializerOptions _serializerOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    public ProjectCrmContext(ISiteCrmContext siteCrmContext, ICrmService service)
    {
        _siteCrmContext = siteCrmContext;
        _service = service;
    }

    public async Task<AhpProjectDto> GetProject(
        string projectId,
        string userId,
        string organisationId,
        string? consortiumId,
        CancellationToken cancellationToken)
    {
        var request = new invln_getahpprojectRequest
        {
            invln_userid = userId,
            invln_accountid = organisationId.TryToGuidAsString(),
            invln_heprojectid = projectId.TryToGuidAsString(),
        };

        return await _service.ExecuteAsync<invln_getahpprojectRequest, invln_getahpprojectResponse, AhpProjectDto>(
            request,
            r => r.invln_ahpProjectApplications,
            cancellationToken);
    }

    public async Task<PagedResponseDto<ProjectDto>> GetProjects(
        string userId,
        string organisationId,
        string? consortiumId,
        PagingRequestDto pagination,
        CancellationToken cancellationToken)
    {
        var sites = await _siteCrmContext.GetUserSites(userId, new PagingRequestDto { pageNumber = 1, pageSize = 100 }, cancellationToken);
        _mockedProjectDtoList[0].Sites = sites.items;

        return new PagedResponseDto<ProjectDto>
        {
            items = pagination.pageNumber == 1 ? _mockedProjectDtoList[..3] : _mockedProjectDtoList[3..],
            totalItemsCount = 4,
            paging = new PagingRequestDto { pageNumber = pagination.pageNumber, pageSize = pagination.pageSize, },
        };
    }

    public async Task<string> CreateProject(
        string userId,
        string organisationId,
        string? consortiumId,
        string frontDoorProjectId,
        string projectName,
        IList<SiteDto> sites,
        CancellationToken cancellationToken)
    {
        var request = new invln_createahpprojectRequest
        {
            invln_userid = userId,
            invln_accountid = organisationId.ToGuidAsString(),
            invln_consortiumid = consortiumId?.ToGuidAsString()!,
            invln_heprojectid = frontDoorProjectId,
            invln_projectname = projectName,
            invln_listofsites = JsonSerializer.Serialize(sites, _serializerOptions),
        };

        return await _service.ExecuteAsync<invln_createahpprojectRequest, invln_createahpprojectResponse>(
            request,
            r => r.invln_ahpprojectid,
            cancellationToken);
    }
}
