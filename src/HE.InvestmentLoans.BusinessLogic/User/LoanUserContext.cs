using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Common.Authorization;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.Contract.User.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.User;

public class LoanUserContext : ILoanUserContext
{
    private readonly IUserContext _userContext;

    private readonly ILoanUserRepository _loanUserRepository;

    private readonly ICacheService _cacheService;

    private IList<UserAccount> _accounts = new List<UserAccount>();

    private UserAccount? _selectedAccount;

    private UserDetails? _details;

    public LoanUserContext(IUserContext userContext, ILoanUserRepository loanUserRepository, ICacheService cacheService)
    {
        _userContext = userContext;
        _loanUserRepository = loanUserRepository;
        _cacheService = cacheService;
    }

    public UserGlobalId UserGlobalId => UserGlobalId.From(_userContext.UserGlobalId);

    public string Email => _userContext.Email;

    public async Task<Guid?> GetSelectedAccountId()
    {
        if (_details is null)
        {
            await LoadUserData();
        }

        return _selectedAccount?.AccountId;
    }

    public async Task<IList<UserAccount>> GetAllAccounts()
    {
        if (_details is null)
        {
            await LoadUserData();
        }

        return _accounts;
    }

    public async Task<UserAccount> GetSelectedAccount()
    {
        if (_details is null)
        {
            await LoadUserData();
        }

        return _selectedAccount!;
    }

    public async Task<UserDetails> GetUserDetails()
    {
        if (_details is null)
        {
            await LoadUserData();
        }

        return _details!;
    }

    public async void RefreshUserData()
    {
        await RefreshUserAccounts();
        await RefreshUserDetails();
    }

    public async Task<bool> IsProfileCompleted()
    {
        if (_details is null)
        {
            await LoadUserData();
        }

        return _details!.IsProfileCompleted();
    }

    public async Task<bool> IsLinkedWithOrganization()
    {
        if (_details is null)
        {
            await LoadUserData();
        }

        return _selectedAccount is not null;
    }

    private async Task RefreshUserDetails()
    {
        _details = await _loanUserRepository.GetUserDetails(UserGlobalId)
                   ?? throw new NotFoundException(nameof(UserDetails), UserGlobalId.ToString());
        _cacheService.SetValue($"UserDetails-{UserGlobalId}", _details);
    }

    private async Task RefreshUserAccounts()
    {
        var userAccounts = await _loanUserRepository.GetUserAccounts(UserGlobalId.From(_userContext.UserGlobalId), _userContext.Email);
        _cacheService.SetValue($"{nameof(RefreshUserAccounts)}_{_userContext.UserGlobalId}", userAccounts);
    }

    private async Task LoadUserData()
    {
        await LoadUserAccounts();
        await LoadUserDetails();
    }

    private async Task LoadUserAccounts()
    {
        _accounts = (await _cacheService.GetValueAsync(
            $"{nameof(LoadUserData)}_{_userContext.UserGlobalId}",
            async () => await _loanUserRepository.GetUserAccounts(
                UserGlobalId.From(_userContext.UserGlobalId),
                _userContext.Email)))!;

        _selectedAccount = _accounts?.MinBy(x => x.AccountId);
    }

    private async Task LoadUserDetails()
    {
        _details = await _cacheService.GetValueAsync(
                       $"UserDetails-{UserGlobalId}",
                       async () => await _loanUserRepository.GetUserDetails(UserGlobalId))
                   ?? throw new NotFoundException(nameof(UserDetails), UserGlobalId.ToString());
    }
}
