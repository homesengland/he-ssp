using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Cache;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.Crm;

public class RequestCacheAllocationCrmContextDecorator : IAllocationCrmContext
{
    private readonly InMemoryCache<AllocationClaimsDto, string> _cache = new();

    private readonly IAllocationCrmContext _decorated;

    public RequestCacheAllocationCrmContextDecorator(IAllocationCrmContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<AllocationClaimsDto> GetById(string id, string organisationId, string userId, CancellationToken cancellationToken)
    {
        return (await _cache.GetFromCache(
            id.ToGuidAsString(),
            async () => await _decorated.GetById(id, organisationId, userId, cancellationToken)))!;
    }

    public async Task Save(string allocationId, PhaseClaimsDto dto, string organisationId, string userId, CancellationToken cancellationToken)
    {
        _cache.Delete(allocationId.ToGuidAsString());

        await _decorated.Save(allocationId, dto, organisationId, userId, cancellationToken);
    }
}
