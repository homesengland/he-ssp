using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Domain.User.Repositories;

public interface IProfileRepository
{
    Task<UserProfileDetails> GetProfileDetails(UserGlobalId userGlobalId);

    Task SaveAsync(UserProfileDetails userProfileDetails, UserGlobalId userGlobalId, CancellationToken cancellationToken);
}
