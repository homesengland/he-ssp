using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Contract.User.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.User;

public interface ILoanUserContext
{
    public UserGlobalId UserGlobalId { get; }

    public string Email { get; }

    public IReadOnlyCollection<string> Roles { get; }

    public Task<Guid> GetSelectedAccountId();

    Task<IList<Guid>> GetAllAccountIds();

    Task<UserAccount> GetSelectedAccount();

    public void RefreshDetails();

    public Task<bool> IsProfileCompleted();

    Task<bool> IsLinkedWithOrganization();
}
