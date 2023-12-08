using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Domain.Data;
using HE.Investments.Account.Shared;

namespace HE.Investments.Account.Domain.Users.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly IUsersCrmContext _usersCrmContext;

    public UsersRepository(IUsersCrmContext usersCrmContext)
    {
        _usersCrmContext = usersCrmContext;
    }

    public async Task<IList<UserDetails>> GetUsers()
    {
        var users = await _usersCrmContext.GetUsers(PortalConstants.LoansPortalType);

        return users
            .Select(u => new UserDetails(u.contactExternalId, u.firstName, u.lastName, u.email, u.jobTitle, null, null))
            .ToList();
    }
}
