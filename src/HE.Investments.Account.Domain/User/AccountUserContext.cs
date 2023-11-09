using HE.Investments.Account.Domain.User.Repositories;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Common.User;

namespace HE.Investments.Account.Domain.User;

public class AccountUserContext : IAccountUserContext
{
    private readonly IUserContext _userContext;

    private readonly ICacheService _cacheService;

    private readonly IUserRepository _userRepository;

    private IList<UserAccount> _accounts = new List<UserAccount>();

    private UserAccount? _selectedAccount;

    public AccountUserContext(IUserRepository userRepository, ICacheService cacheService, IUserContext userContext)
    {
        _userRepository = userRepository;
        _cacheService = cacheService;
        _userContext = userContext;
    }

    public UserGlobalId UserGlobalId => UserGlobalId.From(_userContext.UserGlobalId);

    public string Email => _userContext.Email;

    public async Task RefreshUserData()
    {
        await LoadUserAccounts();
    }

    public async Task<UserAccount> GetSelectedAccount()
    {
        if (_selectedAccount is null)
        {
            await LoadUserAccounts();
        }

        return _selectedAccount!;
    }

    public async Task<bool> IsLinkedWithOrganization()
    {
        if (_selectedAccount is null)
        {
            await LoadUserAccounts();
        }

        return _selectedAccount is not null;
    }

    private async Task LoadUserAccounts()
    {
        _accounts = (await _cacheService.GetValueAsync(
            $"{nameof(UserAccount)}-{_userContext.UserGlobalId}",
            async () => await _userRepository.GetUserAccounts(
                UserGlobalId.From(_userContext.UserGlobalId),
                _userContext.Email)))!;

        _selectedAccount = _accounts?.MinBy(x => x.AccountId);
    }
}
