using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Common.Authorization;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.Contract.Exceptions;

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

    public string UserGlobalId => _userContext.UserGlobalId;

    public string Email => _userContext.Email ?? string.Empty;

    public IReadOnlyCollection<string> Roles { get; }

    public async Task<Guid> GetSelectedAccountId()
    {
        if (_selectedAccount is null)
        {
            await LoadUserDetails();
        }

        return _selectedAccount!.AccountId;
    }

    public async Task<IList<Guid>> GetAllAccountIds()
    {
        if (_selectedAccount is null)
        {
            await LoadUserDetails();
        }

        return _accountIds;
    }

    public async Task<UserAccount> GetSelectedAccount()
    {
        if (_selectedAccount is null)
        {
            await LoadUserDetails();
        }

        return _selectedAccount!;
    }

    private async Task LoadUserDetails()
    {
        const string defaultAccountGuid = "429d11ab-15fe-ed11-8f6c-002248c653e1";

        const string defaultAccountName = "Default account";

        var userDetails = await _cacheService.GetValueAsync($"{nameof(this.LoadUserDetails)}_{_userContext.UserGlobalId}", async () => await _loanUserRepository.GetUserDetails(_userContext.UserGlobalId, _userContext.Email)) ?? throw new LoanUserAccountIsMissingException();

        var accounts = userDetails.contactRoles.OrderBy(x => x.accountId).ToList();

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
