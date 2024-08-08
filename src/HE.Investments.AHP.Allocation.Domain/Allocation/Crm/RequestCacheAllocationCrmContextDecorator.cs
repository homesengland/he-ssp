using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Cache;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.Crm;

public class RequestCacheAllocationCrmContextDecorator : IAllocationCrmContext
{
    private readonly InMemoryCache<AllocationDto, string> _allocationCache = new();

    private readonly InMemoryCache<AllocationClaimsDto, string> _claimsCache = new();

    private readonly IAllocationCrmContext _decorated;

    public RequestCacheAllocationCrmContextDecorator(IAllocationCrmContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<AllocationDto> GetAllocation(string id, string organisationId, string userId, CancellationToken cancellationToken)
    {
        return (await _allocationCache.GetFromCache(
            id.ToGuidAsString(),
            async () => await _decorated.GetAllocation(id, organisationId, userId, cancellationToken)))!;
    }

    public async Task<AllocationClaimsDto> GetAllocationClaims(string id, string organisationId, string userId, CancellationToken cancellationToken)
    {
        return (await _claimsCache.GetFromCache(
            id.ToGuidAsString(),
            async () => await _decorated.GetAllocationClaims(id, organisationId, userId, cancellationToken)))!;
    }

    public async Task SavePhaseClaims(string allocationId, PhaseClaimsDto dto, string organisationId, string userId, CancellationToken cancellationToken)
    {
        _allocationCache.Delete(allocationId.ToGuidAsString());
        _claimsCache.Delete(allocationId.ToGuidAsString());

        await _decorated.SavePhaseClaims(allocationId, dto, organisationId, userId, cancellationToken);
    }
}
