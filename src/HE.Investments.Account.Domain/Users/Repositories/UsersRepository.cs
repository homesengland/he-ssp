using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Domain.Data;

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

        var roles = await _usersCrmContext.GetUsersRole(users.Select(u => u.contactExternalId).ToList());

        return users
            .Select(u => CreateUserDetails(u, r => GetRole(r, roles)))
            .ToList();
    }

    private static UserDetails CreateUserDetails(ContactDto contact, Func<string, int?> getRole)
    {
        var role = UserRoleMapper.ToDomain(getRole(contact.contactExternalId));
        return new UserDetails(contact.contactExternalId, contact.firstName, contact.lastName, contact.email, contact.jobTitle, role, null);
    }

    private static int? GetRole(string id, IDictionary<string, int?> roles)
    {
        return !roles.TryGetValue(id, out var role) ? null : role;
    }
}
