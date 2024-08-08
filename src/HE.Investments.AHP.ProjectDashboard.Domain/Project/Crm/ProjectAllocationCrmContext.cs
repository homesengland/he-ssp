using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Extensions;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Project.Crm;

public class ProjectAllocationCrmContext : IProjectAllocationCrmContext
{
    private readonly ICrmService _service;

    public ProjectAllocationCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task<AhpProjectDto> GetProjectAllocations(string projectId, string userId, string organisationId, string? consortiumId, CancellationToken cancellationToken)
    {
        // return await _service.ExecuteAsync<invln_getprojectallocationsRequest, invln_getprojectallocationsResponse, AhpProjectDto>(
        //     new invln_getahpprojectRequest
        ////     {
        //         invln_userid = userId,
        //         invln_accountid = organisationId.TryToGuidAsString(),
        //         invln_heprojectid = projectId.TryToGuidAsString(),
        //     },
        //     r => r.invln_ahpProjectAllocations,
        ////     cancellationToken); todo crm wire up AB#104052 AB#104051

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
}
