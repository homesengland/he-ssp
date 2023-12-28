using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Shared.User.ValueObjects;

namespace HE.Investments.Account.Domain.Users.Repositories;

public interface IUsersRepository
{
    Task<IList<UserDetails>> GetUsers(OrganisationId organisationId);
}
