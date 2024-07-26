using System.Text.Json;
using System.Text.Json.Serialization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Extensions;
using Microsoft.FeatureManagement;

namespace HE.Investment.AHP.Domain.Project.Crm;

public class ProjectCrmContext : IProjectCrmContext
{
    private readonly ICrmService _service;

    private readonly JsonSerializerOptions _serializerOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    private readonly IFeatureManager _featureManager;

    public ProjectCrmContext(ICrmService service, IFeatureManager featureManager)
    {
        _service = service;
        _featureManager = featureManager;
    }

    public async Task<AhpProjectDto> GetProject(
        string projectId,
        string userId,
        string organisationId,
        string? consortiumId,
        CancellationToken cancellationToken)
    {
        if (await _featureManager.IsEnabledAsync(FeatureFlags.TurnOffAhpAllocation, cancellationToken))
        {
            return await _service.ExecuteAsync<invln_getahpprojectRequest, invln_getahpprojectResponse, AhpProjectDto>(
                new invln_getahpprojectRequest
                {
                    invln_userid = userId,
                    invln_accountid = organisationId.TryToGuidAsString(),
                    invln_consortiumid = consortiumId?.ToGuidAsString()!,
                    invln_heprojectid = projectId.TryToGuidAsString(),
                },
                r => r.invln_ahpProjectApplications,
                cancellationToken);
        }

        return await _service.ExecuteAsync<invln_getahpproject_v2Request, invln_getahpproject_v2Response, AhpProjectDto>(
            new invln_getahpproject_v2Request
            {
                invln_userid = userId,
                invln_accountid = organisationId.TryToGuidAsString(),
                invln_consortiumid = consortiumId?.ToGuidAsString()!,
                invln_heprojectid = projectId.TryToGuidAsString(),
            },
            r => r.invln_ahpProjectApplications,
            cancellationToken);
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
            invln_consortiumid = consortiumId?.TryToGuidAsString()!,
            invln_pagingrequest = CrmResponseSerializer.Serialize(pagination),
        };

        return await _service.ExecuteAsync<invln_getahpprojectsRequest, invln_getahpprojectsResponse, PagedResponseDto<AhpProjectDto>>(
            request,
            r => r.invln_listOfAhpProjects,
            cancellationToken);
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
