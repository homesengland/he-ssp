extern alias Org;

using HE.Investments.Account.Shared.User;

namespace HE.InvestmentLoans.BusinessLogic.User.Repositories;

public interface ILoanUserRepository
{
    public Task<IList<UserAccount>> GetUserAccounts(UserGlobalId userGlobalId, string userEmail);

    public Task<UserDetails> GetUserDetails(UserGlobalId userGlobalId);

    public Task SaveAsync(UserDetails userDetails, UserGlobalId userGlobalId, CancellationToken cancellationToken);
}
