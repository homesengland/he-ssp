using HE.Investments.AHP.Consortium.Contract.Events;
using HE.Investments.AHP.Consortium.Shared.UserContext;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investment.AHP.Domain.UserContext.EventHandlers;

public class InvalidateConsortiumCacheEventHandler : IEventHandler<ConsortiumMemberChangedEvent>
{
    private readonly ICacheService _cacheService;

    public InvalidateConsortiumCacheEventHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task Handle(ConsortiumMemberChangedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _cacheService.DeleteAsync(ConsortiumCacheKeys.OrganisationConsortium(domainEvent.OrganisationId));
    }
}
