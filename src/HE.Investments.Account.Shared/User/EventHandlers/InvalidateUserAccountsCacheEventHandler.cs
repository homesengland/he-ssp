using HE.Investments.Account.Contract.User.Events;
using HE.Investments.Account.Contract.UserOrganisation.Events;
using HE.Investments.Account.Shared.Config;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investments.Account.Shared.User.EventHandlers;

public class InvalidateUserAccountsCacheEventHandler : IEventHandler<UserAccountsChangedEvent>, IEventHandler<UserUnlinkedEvent>
{
    private readonly ICacheService _cacheService;

    private readonly IAccountUserContext _accountUserContext;

    public InvalidateUserAccountsCacheEventHandler(ICacheService cacheService, IAccountUserContext accountUserContext)
    {
        _cacheService = cacheService;
        _accountUserContext = accountUserContext;
    }

    public async Task Handle(UserAccountsChangedEvent notification, CancellationToken cancellationToken)
    {
        await InvalidateCache(notification.UserGlobalId);
    }

    public async Task Handle(UserUnlinkedEvent notification, CancellationToken cancellationToken)
    {
        await InvalidateCache(notification.UserGlobalId);
    }

    private async Task InvalidateCache(UserGlobalId userGlobalId)
    {
        if (_accountUserContext.UserGlobalId == userGlobalId)
        {
            await _accountUserContext.RefreshUserData();
        }
        else
        {
            await _cacheService.DeleteAsync(CacheKeys.UserAccounts(userGlobalId.Value));
        }
    }
}
