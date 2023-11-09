using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Common.Exceptions;
using HE.Investments.Account.Domain.User;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;

namespace HE.InvestmentLoans.BusinessLogic.User;

public class LoanUserContext : ILoanUserContext
{
    private readonly IAccountUserContext _accountUserContext;

    private readonly ILoanUserRepository _loanUserRepository;

    private readonly ICacheService _cacheService;

    private UserDetails? _details;

    public LoanUserContext(ILoanUserRepository loanUserRepository, ICacheService cacheService, IAccountUserContext accountUserContext)
    {
        _loanUserRepository = loanUserRepository;
        _cacheService = cacheService;
        _accountUserContext = accountUserContext;
    }

    public UserGlobalId UserGlobalId => _accountUserContext.UserGlobalId;

    public string Email => _accountUserContext.Email;

    public async Task<Guid?> GetSelectedAccountId()
    {
        var selectedAccount = await _accountUserContext.GetSelectedAccount();

        return selectedAccount.AccountId;
    }

    public async Task<UserAccount> GetSelectedAccount()
    {
        return await _accountUserContext.GetSelectedAccount();
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
        return await _accountUserContext.IsLinkedWithOrganization();
    }

    private async Task RefreshUserDetails()
    {
        _details = await _loanUserRepository.GetUserDetails(UserGlobalId)
                   ?? throw new NotFoundException(nameof(UserDetails), UserGlobalId.ToString());
        _cacheService.SetValue($"{nameof(UserDetails)}-{UserGlobalId}", _details);
    }

    private async Task LoadUserData()
    {
        await LoadUserDetails();
    }

    private async Task LoadUserDetails()
    {
        _details = await _cacheService.GetValueAsync(
                       $"{nameof(UserDetails)}-{UserGlobalId}",
                       async () => await _loanUserRepository.GetUserDetails(UserGlobalId))
                   ?? throw new NotFoundException(nameof(UserDetails), UserGlobalId.ToString());
    }
}
