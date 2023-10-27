using HE.Investments.Account.Domain.User.Entities;
using HE.Investments.Account.Domain.User.ValueObjects;

namespace HE.Investments.Account.Domain.User.Repositories;

public interface IUserRepository
{
    Task<UserDetails> GetUserDetails(UserGlobalId userGlobalId);

    Task SaveAsync(UserDetails userDetails, UserGlobalId userGlobalId, CancellationToken cancellationToken);
}
