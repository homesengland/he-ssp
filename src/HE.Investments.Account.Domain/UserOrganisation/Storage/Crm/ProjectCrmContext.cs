using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Extensions;

namespace HE.Investments.Account.Domain.UserOrganisation.Storage.Crm;

public class ProjectCrmContext : IProjectContext
{
    private readonly ICrmService _service;

    public ProjectCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task<IList<FrontDoorProjectDto>> GetOrganisationProjects(string organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getmultiplefrontdoorprojectsRequest
        {
            invln_organisationid = organisationId.TryToGuidAsString(),
            invln_usehetables = "true",
        };

        return await GetProjects(request, cancellationToken);
    }

    public async Task<IList<FrontDoorProjectDto>> GetUserProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getmultiplefrontdoorprojectsRequest
        {
            inlvn_userid = userGlobalId,
            invln_organisationid = organisationId.TryToGuidAsString(),
            invln_usehetables = "true",
        };

        return await GetProjects(request, cancellationToken);
    }

    private async Task<IList<FrontDoorProjectDto>> GetProjects(invln_getmultiplefrontdoorprojectsRequest request, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_getmultiplefrontdoorprojectsRequest, invln_getmultiplefrontdoorprojectsResponse, IList<FrontDoorProjectDto>>(
            request,
            r => string.IsNullOrEmpty(r.invln_frontdoorprojects) ? "[]" : r.invln_frontdoorprojects,
            cancellationToken);
    }
}
