using HE.Investments.Account.Domain.User.Entities;
using HE.Investments.Account.Domain.User.ValueObjects;

namespace HE.Investments.Account.Domain.User.Repositories;

public class UserRepository : IUserRepository
{
    public Task<UserDetails> GetUserDetails(UserGlobalId userGlobalId)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(UserDetails userDetails, UserGlobalId userGlobalId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
