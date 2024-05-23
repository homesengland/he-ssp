using System.Text.Json;
using System.Text.Json.Serialization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Domain.Application.Crm;
using HE.Investment.AHP.Domain.Site.Crm;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.CRM.Services;

namespace HE.Investment.AHP.Domain.Project.Crm;

public class ProjectCrmContext : IProjectCrmContext
{
    private readonly ProjectDto _mockedProjectDto =
        new() { ProjectId = LegacyProject.ProjectId, ProjectName = LegacyProject.ProjectName, };

    private readonly IApplicationCrmContext _applicationCrmContext;

    private readonly ISiteCrmContext _siteCrmContext;

    private readonly ICrmService _service;

    private readonly JsonSerializerOptions _serializerOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    public ProjectCrmContext(IApplicationCrmContext applicationCrmContext, ISiteCrmContext siteCrmContext, ICrmService service)
    {
        _applicationCrmContext = applicationCrmContext;
        _siteCrmContext = siteCrmContext;
        _service = service;
    }

    public async Task<ProjectDto> GetProject(
        string projectId,
        string userId,
        string organisationId,
        string? consortiumId,
        CancellationToken cancellationToken)
    {
        var applications = await _applicationCrmContext.GetUserApplications(organisationId, userId, cancellationToken);
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

    public async Task<PagedResponseDto<AhpProjectDto>> GetProjects(
        string userId,
        string organisationId,
        string? consortiumId,
        PagingRequestDto pagination,
        CancellationToken cancellationToken)
    {
        var request = new invln_getahpprojectsRequest
        {
            invln_userid = userId,
            invln_accountid = organisationId,
            invln_consortiumid = null,
            invln_pagingrequest = CrmResponseSerializer.Serialize(pagination),
        };

        return await _service.ExecuteAsync<invln_getahpprojectsRequest, invln_getahpprojectsResponse, PagedResponseDto<AhpProjectDto>>(
            request,
            r => r.invln_listOfAhpProjects,
            cancellationToken);
    }

    public async Task<ProjectSitesDto> GetProjectSites(
        string projectId,
        string userId,
        string organisationId,
        string? consortiumId,
        CancellationToken cancellationToken)
    {
        var sites = await _siteCrmContext.GetUserSites(userId, new PagingRequestDto { pageNumber = 1, pageSize = 100 }, cancellationToken);
        return new ProjectSitesDto { ProjectId = _mockedProjectDto.ProjectId, ProjectName = _mockedProjectDto.ProjectName, Sites = sites.items, };
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
            invln_accountid = organisationId,
            invln_consortiumid = consortiumId ?? string.Empty,
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
