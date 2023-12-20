using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Shared.Repositories;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Common.User;
using HE.Investments.Loans.Common.Exceptions;

namespace HE.Investments.Account.Shared;

public class AccountUserContext : IAccountUserContext
{
    private readonly IUserContext _userContext;

    private readonly ICacheService _cacheService;

    private readonly IAccountRepository _accountRepository;

    private IList<UserAccount> _accounts = new List<UserAccount>();

    private UserAccount? _selectedAccount;

    private UserProfileDetails? _userProfile;

    public AccountUserContext(IAccountRepository accountRepository, ICacheService cacheService, IUserContext userContext)
    {
        _accountRepository = accountRepository;
        _cacheService = cacheService;
        _userContext = userContext;
    }

    public bool IsLogged => _userContext.IsAuthenticated;

    public UserGlobalId UserGlobalId => UserGlobalId.From(_userContext.UserGlobalId);

    public string Email => _userContext.Email;

    public async Task RefreshAccounts()
    {
        _accounts = await _accountRepository.GetUserAccounts(
            UserGlobalId.From(_userContext.UserGlobalId),
            _userContext.Email)!;

        _selectedAccount = _accounts.MinBy(x => x.AccountId);
        await _cacheService.SetValueAsync($"{nameof(UserAccount)}-{_userContext.UserGlobalId}", _accounts);
    }

    public async Task<UserAccount> GetSelectedAccount()
    {
        if (_selectedAccount is null)
        {
            await LoadUserAccounts();
        }

        return _selectedAccount!;
    }

    public async Task<bool> IsProfileCompleted()
    {
        if (_userProfile is null)
        {
            await LoadProfileDetails();
        }

        return _userProfile!.IsCompleted();
    }

    public async Task<bool> IsLinkedWithOrganisation()
    {
        if (_selectedAccount is null)
        {
            await LoadUserAccounts();
        }

        return _selectedAccount is not null;
    }

    public async Task<bool> HasOneOfRole(UserRole[] roles)
    {
        var selectedAccount = await GetSelectedAccount();
        return roles.Contains(selectedAccount.Role());
    }

    public async Task LoadUserAccounts()
    {
        _accounts = (await _cacheService.GetValueAsync(
            $"{nameof(UserAccount)}-{_userContext.UserGlobalId}",
            async () => await _accountRepository.GetUserAccounts(
                UserGlobalId.From(_userContext.UserGlobalId),
                _userContext.Email)))!;

        _selectedAccount = _accounts?.MinBy(x => x.AccountId);
    }

    public async Task<UserProfileDetails> GetProfileDetails()
    {
        if (_userProfile is null)
        {
            await LoadProfileDetails();
        }

        return _userProfile!;
    }

    public async Task RefreshProfileDetails()
    {
        _userProfile = await _accountRepository.GetProfileDetails(UserGlobalId)
                       ?? throw new NotFoundException(nameof(UserProfileDetails), UserGlobalId.ToString());

        await _cacheService.SetValueAsync($"{nameof(UserProfileDetails)}-{UserGlobalId}", _userProfile);
    }

    private async Task LoadProfileDetails()
    {
        await LoadUserAccounts();
        _userProfile = await _cacheService.GetValueAsync(
                           $"{nameof(UserProfileDetails)}-{UserGlobalId}",
                           async () => await _accountRepository.GetProfileDetails(UserGlobalId))
                       ?? throw new NotFoundException(nameof(UserProfileDetails), UserGlobalId.ToString());
    }
}
