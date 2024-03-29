using HE.Investments.Account.Contract.User.Events;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Config;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investments.Account.Domain.User.EventHandlers;

public class InvalidateUserProfileCacheEventHandler : IEventHandler<UserProfileChangedEvent>
{
    private readonly ICacheService _cacheService;

    private readonly IAccountUserContext _accountUserContext;

    public InvalidateUserProfileCacheEventHandler(ICacheService cacheService, IAccountUserContext accountUserContext)
    {
        _cacheService = cacheService;
        _accountUserContext = accountUserContext;
    }

    public async Task Handle(UserProfileChangedEvent notification, CancellationToken cancellationToken)
    {
        if (_accountUserContext.UserGlobalId == notification.UserGlobalId)
        {
            await _accountUserContext.RefreshUserData();
        }
        else
        {
            await _cacheService.DeleteAsync(AccountCacheKeys.ProfileDetails(notification.UserGlobalId.Value));
        }
    }
}
