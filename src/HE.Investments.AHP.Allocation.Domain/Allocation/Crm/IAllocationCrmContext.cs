using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.Crm;

public interface IAllocationCrmContext
{
    Task<AllocationClaimsDto> GetById(string id, string organisationId, string userId, CancellationToken cancellationToken);
}
