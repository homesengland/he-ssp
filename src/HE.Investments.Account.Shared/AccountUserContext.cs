using HE.Investments.Account.Shared.Config;
using HE.Investments.Account.Shared.Repositories;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Infrastructure.Cache;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Common.User;

namespace HE.Investments.Account.Shared;

public class AccountUserContext : IAccountUserContext
{
    private readonly IUserContext _userContext;

    private readonly CachedEntity<UserProfileDetails> _userProfile;

    private readonly CachedEntity<IList<UserAccount>> _userAccounts;

    public AccountUserContext(IAccountRepository accountRepository, ICacheService cacheService, IUserContext userContext)
    {
        _userContext = userContext;
        _userProfile = new CachedEntity<UserProfileDetails>(
            cacheService,
            AccountCacheKeys.ProfileDetails(_userContext.UserGlobalId),
            async () => await accountRepository.GetProfileDetails(UserGlobalId) ??
                        throw new NotFoundException(nameof(UserProfileDetails), UserGlobalId.ToString()));
        _userAccounts = new CachedEntity<IList<UserAccount>>(
            cacheService,
            AccountCacheKeys.UserAccounts(_userContext.UserGlobalId),
            async () => await accountRepository.GetUserAccounts(UserGlobalId, _userContext.Email));
    }

    public bool IsLogged => _userContext.IsAuthenticated;

    public UserGlobalId UserGlobalId => UserGlobalId.From(_userContext.UserGlobalId);

    public string Email => _userContext.Email;

    public async Task<UserAccount> GetSelectedAccount()
    {
        var accounts = await _userAccounts.GetAsync();
        return accounts?.MinBy(x => x.Organisation?.OrganisationId.Value) ?? throw new NotFoundException(nameof(UserAccount));
    }

    public async Task RefreshUserData()
    {
        await _userAccounts.InvalidateAsync();
        await _userProfile.InvalidateAsync();
    }

    public async Task<bool> IsLinkedWithOrganisation()
    {
        var accounts = await _userAccounts.GetAsync();
        return accounts != null && accounts.Any();
    }

    public async Task<bool> IsProfileCompleted()
    {
        await _userAccounts.GetAsync();
        var userProfile = await _userProfile.GetAsync();
        return userProfile!.IsCompleted();
    }

    public async Task<UserProfileDetails> GetProfileDetails()
    {
        var userProfile = await _userProfile.GetAsync();
        return userProfile!;
    }
}
