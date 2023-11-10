using HE.Investments.Account.Domain.User.Entities;
using HE.Investments.Account.Shared.User;

namespace HE.Investments.Account.Domain.User.Repositories;

public interface IUserRepository
{
    Task<IList<UserAccount>> GetUserAccounts(UserGlobalId userGlobalId, string userEmail);

    Task<UserProfileDetails> GetUserProfileInformation(UserGlobalId userGlobalId);

    Task SaveAsync(UserProfileDetails userProfileDetails, UserGlobalId userGlobalId, CancellationToken cancellationToken);
}
