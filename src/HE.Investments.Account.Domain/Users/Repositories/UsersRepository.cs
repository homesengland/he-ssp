using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Domain.Data;
using HE.Investments.Account.Domain.Data.Extensions;

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
        var users = await _usersCrmContext.GetUsers();
        if (!users.Any())
        {
            return new List<UserDetails>();
        }

        return users
            .Where(x => x.IsConnectedWithExternalIdentity())
            .Select(CreateUserDetails)
            .ToList();
    }

    private static UserDetails CreateUserDetails(ContactDto contact)
    {
        var role = UserRoleMapper.ToDomain(contact.webrole);
        return new UserDetails(contact.contactExternalId, contact.firstName, contact.lastName, contact.email, contact.jobTitle, role, null);
    }
}
