using HE.Investments.Account.Domain.User.Entities;
using HE.Investments.Account.Domain.User.ValueObjects;

namespace HE.Investments.Account.Domain.User.Repositories;

public interface IUserRepository
{
    Task<UserProfileDetails> GetUserProfileInformation(UserGlobalId userGlobalId);

    Task SaveAsync(UserProfileDetails userProfileDetails, UserGlobalId userGlobalId, CancellationToken cancellationToken);
}
