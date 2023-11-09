using HE.Investments.Account.Shared.User;

namespace HE.InvestmentLoans.BusinessLogic.User;

public interface ILoanUserContext
{
    public UserGlobalId UserGlobalId { get; }

    public string Email { get; }

    public Task<Guid?> GetSelectedAccountId();

    Task<UserAccount> GetSelectedAccount();

    Task<UserDetails> GetUserDetails();

    public void RefreshUserData();

    public Task<bool> IsProfileCompleted();

    Task<bool> IsLinkedWithOrganization();
}
