extern alias Org;

using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.CRM.Services;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.FrontDoor.Shared.Project.Crm;

internal class ProjectCrmContext : IProjectCrmContext
{
    private readonly ICrmService _service;

    public ProjectCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getsinglefrontdoorprojectRequest
        {
            invln_organisationid = organisationId.ToString(),
            invln_frontdoorprojectid = projectId,
            invln_fieldstoretrieve = ProjectCrmFields.ProjectToRead.FormatFields(),
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
            invln_fieldstoretrieve = ProjectCrmFields.ProjectToRead.FormatFields(),
        };

        return await GetProject(request, cancellationToken);
    }

    public async Task<FrontDoorProjectSiteDto?> GetProjectSite(string projectId, CancellationToken cancellationToken)
    {
        var request = new invln_getmultiplefrontdoorprojectssiteRequest
        {
            invln_frontdoorprojectid = projectId,
            invln_pagingrequest = CrmResponseSerializer.Serialize(new PagingRequestDto { pageNumber = 1, pageSize = 1 }),
            invln_fieldstoretrieve = ProjectSiteCrmFields.SiteToRead.FormatFields(),
        };

        var sites = await _service
            .ExecuteAsync<
                invln_getmultiplefrontdoorprojectssiteRequest,
                invln_getmultiplefrontdoorprojectssiteResponse,
                PagedResponseDto<FrontDoorProjectSiteDto>>(
                request,
                r => r.invln_frontdoorprojectsites,
                cancellationToken);

        return sites.items.FirstOrDefault();
    }

    public async Task DeactivateProject(string projectId, CancellationToken cancellationToken)
    {
        var request = new invln_deactivatefrontdoorprojectRequest { invln_frontdoorprojectid = projectId };

        await _service.ExecuteAsync<invln_deactivatefrontdoorprojectRequest, invln_deactivatefrontdoorprojectResponse>(
            request,
            r => r.invln_projectdeactivated,
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
