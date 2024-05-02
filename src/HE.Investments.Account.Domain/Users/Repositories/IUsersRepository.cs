using HE.Investments.Account.Contract.Users;
using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Domain.Users.Repositories;

public interface IUsersRepository
{
    Task<IList<UserDetails>> GetUsers(OrganisationId organisationId);
}
