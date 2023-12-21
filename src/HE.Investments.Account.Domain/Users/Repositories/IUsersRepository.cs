using HE.Investments.Account.Contract.Users;

namespace HE.Investments.Account.Domain.Users.Repositories;

public interface IUsersRepository
{
    Task<IList<UserDetails>> GetUsers(Guid organisationId);
}
