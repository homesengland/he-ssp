using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.CRM.Services;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.Crm;

public class AllocationCrmContext : IAllocationCrmContext
{
    private readonly ICrmService _service;

    public AllocationCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task<AllocationDto> GetAllocation(string id, string organisationId, string userId, CancellationToken cancellationToken)
    {
        var request = new invln_getallocationRequest
        {
            invln_userid = userId,
            invln_accountid = ShortGuid.ToGuid(organisationId),
            invln_allocationid = ShortGuid.ToGuid(id),
        };

        return await _service.ExecuteAsync<invln_getallocationRequest, invln_getallocationResponse, AllocationDto>(
            request,
            r => r.invln_ahpallocation,
            cancellationToken);
    }

    public async Task<AllocationClaimsDto> GetAllocationClaims(string id, string organisationId, string userId, CancellationToken cancellationToken)
    {
        var request = new invln_getallocationclaimsRequest
        {
            invln_userid = userId,
            invln_accountid = ShortGuid.ToGuid(organisationId),
            invln_allocationid = ShortGuid.ToGuid(id),
        };

        return await _service.ExecuteAsync<invln_getallocationclaimsRequest, invln_getallocationclaimsResponse, AllocationClaimsDto>(
            request,
            r => r.invln_ahpallocationclaims,
            cancellationToken);
    }

    public async Task SavePhaseClaims(string allocationId, PhaseClaimsDto dto, string organisationId, string userId, CancellationToken cancellationToken)
    {
        var request = new invln_setallocationphaseRequest
        {
            invln_userid = userId,
            invln_accountid = ShortGuid.ToGuid(organisationId),
            invln_allocationid = ShortGuid.ToGuid(allocationId),
            invln_deliveryphaseid = ShortGuid.ToGuid(dto.Id),
            invln_phaseclaimsdto = CrmResponseSerializer.Serialize(dto),
        };

        await _service.ExecuteAsync<invln_setallocationphaseRequest, invln_setallocationphaseResponse, Guid>(
            request,
            r => r.id,
            cancellationToken);
    }
}
