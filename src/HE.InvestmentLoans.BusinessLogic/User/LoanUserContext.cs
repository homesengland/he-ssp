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

    private readonly IList<Guid> _accountIds = new List<Guid>();

    private UserAccount? _selectedAccount;

    public LoanUserContext(IUserContext userContext, ILoanUserRepository loanUserRepository, ICacheService cacheService)
    {
        _userContext = userContext;
        _loanUserRepository = loanUserRepository;
        _cacheService = cacheService;
    }

    public UserGlobalId UserGlobalId => UserGlobalId.From(_userContext.UserGlobalId);

    public string Email => _userContext.Email ?? string.Empty;

    public IReadOnlyCollection<string> Roles { get; }

    public async Task<Guid> GetSelectedAccountId()
    {
        if (_selectedAccount is null)
        {
            await LoadUserAccount();
        }

        return _selectedAccount!.AccountId;
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

    public async void RefreshDetails()
    {
        var userDetails = await _loanUserRepository.GetUserDetails(UserGlobalId)
                                ?? throw new NotFoundException(nameof(UserDetails), UserGlobalId.ToString());

        _cacheService.SetValue(UserGlobalId.ToString(), userDetails);
    }

    public async Task<bool> IsProfileCompleted()
    {
        var selectedUser = await GetSelectedAccount();
        var userDetails = await _cacheService.GetValueAsync(
                                                selectedUser.UserGlobalId.ToString(),
                                                async () => await _loanUserRepository.GetUserDetails(selectedUser.UserGlobalId))
                                                            ?? throw new NotFoundException(nameof(UserDetails), selectedUser.UserGlobalId.ToString());
        return userDetails.IsProfileCompleted();
    }

    private async Task LoadUserAccount()
    {
        var userAccount = await _cacheService.GetValueAsync(
            $"{nameof(this.LoadUserAccount)}_{_userContext.UserGlobalId}",
            async () => await _loanUserRepository.GetUserAccount(UserGlobalId.From(_userContext.UserGlobalId), _userContext.Email))
            ?? throw new LoanUserAccountIsMissingException();

        var accounts = userAccount.contactRoles.OrderBy(x => x.accountId).ToList();

        _accountIds.AddRange(accounts.Select(x => x.accountId).ToList());

        var selectedAccount = accounts.FirstOrDefault() ?? throw new LoanUserAccountIsMissingException();

        _selectedAccount = new UserAccount(UserGlobalId, Email, selectedAccount.accountId, selectedAccount.accountName);
    }
}
