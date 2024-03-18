extern alias Org;

using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Model;
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

    public async Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string userGlobalId, Guid organisationId, CancellationToken cancellationToken)
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
