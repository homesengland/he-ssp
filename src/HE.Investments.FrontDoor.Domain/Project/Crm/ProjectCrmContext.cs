using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.CRM.Services;

namespace HE.Investments.FrontDoor.Domain.Project.Crm;

public class ProjectCrmContext : IProjectCrmContext
{
    private readonly ICrmService _service;

    public ProjectCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task<IList<FrontDoorProjectDto>> GetOrganisationProjects(string userGlobalId, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getmultiplefrontdoorprojectsRequest
        {
            invln_organisationid = organisationId.ToString(),
            inlvn_userid = userGlobalId,
        };
        return await GetProjects(request, cancellationToken);
    }

    public async Task<IList<FrontDoorProjectDto>> GetUserProjects(string userGlobalId, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getmultiplefrontdoorprojectsRequest
        {
            inlvn_userid = userGlobalId,
            invln_organisationid = organisationId.ToString(),
        };

        return await GetProjects(request, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string userGlobalId, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getsinglefrontdoorprojectRequest
        {
            invln_userid = userGlobalId,
            invln_organisationid = organisationId.ToString(),
            invln_frontdoorprojectid = projectId,
        };

        return await GetProject(request, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetUserProjectById(string projectId, string userGlobalId, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getsinglefrontdoorprojectRequest
        {
            invln_organisationid = organisationId.ToString(),
            invln_userid = userGlobalId,
            invln_frontdoorprojectid = projectId,
        };

        return await GetProject(request, cancellationToken);
    }

    public async Task<string> Save(FrontDoorProjectDto dto, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var request = new invln_setfrontdoorprojectRequest
        {
            invln_userid = userAccount.UserGlobalId.ToString(),
            invln_organisationid = userAccount.SelectedOrganisationId().ToString(),
            invln_entityfieldsparameters = CrmResponseSerializer.Serialize(dto),
            invln_frontdoorprojectid = dto.ProjectId,
        };

        return await _service.ExecuteAsync<invln_setfrontdoorprojectRequest, invln_setfrontdoorprojectResponse>(
            request,
            r => r.invln_frontdoorprojectid,
            cancellationToken);
    }

    private async Task<IList<FrontDoorProjectDto>> GetProjects(invln_getmultiplefrontdoorprojectsRequest request, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_getmultiplefrontdoorprojectsRequest, invln_getmultiplefrontdoorprojectsResponse, IList<FrontDoorProjectDto>>(
            request,
            r => string.IsNullOrEmpty(r.invln_frontdoorprojects) ? "[]" : r.invln_frontdoorprojects,
            cancellationToken);
    }

    private async Task<FrontDoorProjectDto> GetProject(
        invln_getsinglefrontdoorprojectRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _service.ExecuteAsync<invln_getsinglefrontdoorprojectRequest, invln_getsinglefrontdoorprojectResponse, IList<FrontDoorProjectDto>>(
            request,
            r => string.IsNullOrEmpty(r.invln_retrievedfrontdoorprojectfields) ? "[]" : r.invln_retrievedfrontdoorprojectfields,
            cancellationToken);

        if (!response.Any())
        {
            throw new NotFoundException("Project", request.invln_frontdoorprojectid);
        }

        return response[0];
    }
}
