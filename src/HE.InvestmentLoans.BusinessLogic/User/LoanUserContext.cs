using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Common.Authorization;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.Common.Utils.Constants.Enums;
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

    public void SaveUserDetailsStatusInCache(UserGlobalId userGlobalId)
    {
        _cacheService.SetValue(userGlobalId.ToString(), ProfileCompletionStatus.Complete);
    }

    public ProfileCompletionStatus? GetUserDetailsStatusFromCache(UserGlobalId userGlobalId)
    {
        return _cacheService.GetValue<ProfileCompletionStatus>(userGlobalId.ToString());
    }

    public async Task<bool> IsProfileCompleted()
    {
        var selectedUser = await GetSelectedAccount();
        var userDetails = await _loanUserRepository.GetUserDetails(selectedUser.UserGlobalId);

        return userDetails.IsProfileComplete();
    }

    private async Task LoadUserAccount()
    {
        const string defaultAccountGuid = "429d11ab-15fe-ed11-8f6c-002248c653e1";

        const string defaultAccountName = "Default account";

        var userAccount = await _cacheService.GetValueAsync($"{nameof(this.LoadUserAccount)}_{_userContext.UserGlobalId}", async () => await _loanUserRepository.GetUserAccount(UserGlobalId.From(_userContext.UserGlobalId), _userContext.Email)) ?? throw new LoanUserAccountIsMissingException();

        var accounts = userAccount.contactRoles.OrderBy(x => x.accountId).ToList();

        _accountIds.AddRange(accounts.Select(x => x.accountId).ToList());

        var selectedAccount = accounts.FirstOrDefault();

        if (selectedAccount is null)
        {
            _selectedAccount = new UserAccount(UserGlobalId, Email, Guid.Parse(defaultAccountGuid), defaultAccountName);
        }
        else
        {
            _selectedAccount = new UserAccount(UserGlobalId, Email, selectedAccount.accountId, selectedAccount.accountName);
        }
    }
}
