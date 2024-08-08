using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.Crm;

public interface IAllocationCrmContext
{
    Task<AllocationDto> GetAllocation(string id, string organisationId, string userId, CancellationToken cancellationToken);

    Task<AllocationClaimsDto> GetAllocationClaims(string id, string organisationId, string userId, CancellationToken cancellationToken);

    Task SavePhaseClaims(string allocationId, PhaseClaimsDto dto, string organisationId, string userId, CancellationToken cancellationToken);
}
