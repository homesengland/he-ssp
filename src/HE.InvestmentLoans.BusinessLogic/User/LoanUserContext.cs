using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Common.Authorization;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.Contract.User.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.User;

public class LoanUserContext : ILoanUserContext
{
    private readonly IUserContext _userContext;

    private readonly ILoanUserRepository _loanUserRepository;

    private readonly ICacheService _cacheService;

    private readonly IList<Guid> _accountIds = new List<Guid>();

    private UserAccount? _selectedAccount;

    private bool? _isLinkedWithOrganization;

    public LoanUserContext(IUserContext userContext, ILoanUserRepository loanUserRepository, ICacheService cacheService)
    {
        _userContext = userContext;
        _loanUserRepository = loanUserRepository;
        _cacheService = cacheService;
    }

    public UserGlobalId UserGlobalId => UserGlobalId.From(_userContext.UserGlobalId);

    public string Email => _userContext.Email ?? string.Empty;

    public IReadOnlyCollection<string> Roles { get; }

    public async Task<Guid?> GetSelectedAccountId()
    {
        if (_selectedAccount is null)
        {
            await LoadUserAccount();
        }

        return _selectedAccount?.AccountId;
    }

    public async Task<IList<Guid>> GetAllAccountIds()
    {
        if (_selectedAccount is null)
        {
            await LoadUserAccount();
        }

        return _accountIds;
    }

    public async Task<UserAccount> GetSelectedAccount()
    {
        if (_selectedAccount is null)
        {
            await LoadUserAccount();
        }

        return _selectedAccount!;
    }

    public async void RefreshUserData()
    {
        var userRoleDto = await _loanUserRepository.GetUserRoles(UserGlobalId.From(_userContext.UserGlobalId), _userContext.Email);

        var userDetails = await _loanUserRepository.GetUserDetails(UserGlobalId)
                                ?? throw new NotFoundException(nameof(UserDetails), UserGlobalId.ToString());

        _cacheService.SetValue($"{nameof(this.LoadUserAccount)}_{_userContext.UserGlobalId}", userRoleDto);

        _cacheService.SetValue($"UserDetails-{UserGlobalId}", userDetails);
    }

    public async Task<bool> IsProfileCompleted()
    {
        var userDetails = await _cacheService.GetValueAsync(
                                                $"UserDetails-{UserGlobalId}",
                                                async () => await _loanUserRepository.GetUserDetails(UserGlobalId))
                                                            ?? throw new NotFoundException(nameof(UserDetails), UserGlobalId.ToString());
        return userDetails.IsProfileCompleted();
    }

    public async Task<bool> IsLinkedWithOrganization()
    {
        if (_isLinkedWithOrganization is null)
        {
            await LoadUserAccount();
        }

        return _isLinkedWithOrganization!.Value;
    }

    private async Task LoadUserAccount()
    {
        var userRoleDto = await _cacheService.GetValueAsync(
            $"{nameof(this.LoadUserAccount)}_{_userContext.UserGlobalId}",
            async () => await _loanUserRepository.GetUserRoles(UserGlobalId.From(_userContext.UserGlobalId), _userContext.Email));

        var userDetails = await _cacheService.GetValueAsync(
                                                $"UserDetails-{UserGlobalId}",
                                                async () => await _loanUserRepository.GetUserDetails(UserGlobalId))
                                                            ?? throw new NotFoundException(nameof(UserDetails), UserGlobalId.ToString());

        var accounts = userRoleDto?.contactRoles?.OrderBy(x => x.accountId).ToList();

        _accountIds.AddRange(accounts?.Select(x => x.accountId).ToList());

        var selectedAccount = accounts?.FirstOrDefault();

        _isLinkedWithOrganization = userRoleDto?.contactRoles?.Any() ?? false;

        _selectedAccount = new UserAccount(
            UserGlobalId,
            Email,
            selectedAccount?.accountId,
            selectedAccount?.accountName,
            userDetails.FirstName,
            userDetails.Surname,
            userDetails.TelephoneNumber);
    }
}
