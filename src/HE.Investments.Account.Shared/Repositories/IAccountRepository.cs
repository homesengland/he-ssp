using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;

namespace HE.Investments.Account.Shared.Repositories;

public interface IAccountRepository
{
    Task<IList<UserAccount>> GetUserAccounts(UserGlobalId userGlobalId, string userEmail);

    Task<UserProfileDetails> GetProfileDetails(UserGlobalId userGlobalId);
}
