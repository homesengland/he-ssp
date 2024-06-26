using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Extensions;

namespace HE.Investments.FrontDoor.Shared.Project.Storage.Crm;

internal sealed class ProjectCrmContext : IProjectContext
{
    private readonly ICrmService _service;

    public ProjectCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string organisationId, CancellationToken cancellationToken, bool? includeInactive = null)
    {
        var request = new invln_getsinglefrontdoorprojectRequest
        {
            invln_organisationid = organisationId.TryToGuidAsString(),
            invln_frontdoorprojectid = projectId.ToGuidAsString(),
            invln_fieldstoretrieve = ProjectCrmFields.ProjectToRead.FormatFields(),
            invln_usehetables = "true",
        };

        return await GetProject(request, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetUserProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken, bool? includeInactive = null)
    {
        var request = new invln_getsinglefrontdoorprojectRequest
        {
            invln_organisationid = organisationId.TryToGuidAsString(),
            invln_userid = userGlobalId,
            invln_frontdoorprojectid = projectId.ToGuidAsString(),
            invln_fieldstoretrieve = ProjectCrmFields.ProjectToRead.FormatFields(),
            invln_usehetables = "true",
        };

        return await GetProject(request, cancellationToken);
    }

    public async Task<FrontDoorProjectSiteDto> GetProjectSite(string projectId, string siteId, CancellationToken cancellationToken)
    {
        var request = new invln_getsinglefrontdoorprojectsiteRequest
        {
            invln_frontdoorprojectsiteid = siteId.ToGuidAsString(),
            invln_frontdoorprojectid = projectId.ToGuidAsString(),
            invln_fieldstoretrieve = ProjectSiteCrmFields.SiteToRead.FormatFields(),
            invln_usehetables = "true",
        };

        return await _service.ExecuteAsync<invln_getsinglefrontdoorprojectsiteRequest, invln_getsinglefrontdoorprojectsiteResponse, FrontDoorProjectSiteDto>(
            request,
            r => r.invln_frontdoorprojectsite,
            cancellationToken);
    }

    public async Task<PagedResponseDto<FrontDoorProjectSiteDto>> GetProjectSites(string projectId, CancellationToken cancellationToken)
    {
        var request = new invln_getmultiplefrontdoorprojectssiteRequest
        {
            invln_frontdoorprojectid = projectId.ToGuidAsString(),
            invln_pagingrequest = CrmResponseSerializer.Serialize(new PagingRequestDto { pageNumber = 1, pageSize = 100 }),
            invln_fieldstoretrieve = ProjectSiteCrmFields.SiteToRead.FormatFields(),
            invln_usehetables = "true",
        };

        return await _service
            .ExecuteAsync<
                invln_getmultiplefrontdoorprojectssiteRequest,
                invln_getmultiplefrontdoorprojectssiteResponse,
                PagedResponseDto<FrontDoorProjectSiteDto>>(
                request,
                r => r.invln_frontdoorprojectsites,
                cancellationToken);
    }

    public async Task DeactivateProject(string projectId, CancellationToken cancellationToken)
    {
        var request = new invln_deactivatefrontdoorprojectRequest
        {
            invln_frontdoorprojectid = projectId.ToGuidAsString(),
            invln_usehetables = "true",
        };

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
