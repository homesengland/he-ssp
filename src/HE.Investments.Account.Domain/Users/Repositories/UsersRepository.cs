using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Domain.Data;
using HE.Investments.Account.Domain.Data.Extensions;
using HE.Investments.Account.Shared.Repositories;
using HE.Investments.Common.Contract;
using OrganisationId = HE.Investments.Account.Shared.User.ValueObjects.OrganisationId;

namespace HE.Investments.Account.Domain.Users.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly IUsersCrmContext _usersCrmContext;

    public UsersRepository(IUsersCrmContext usersCrmContext)
    {
        _usersCrmContext = usersCrmContext;
    }

    public async Task<IList<UserDetails>> GetUsers(OrganisationId organisationId)
    {
        var users = await _usersCrmContext.GetUsers(organisationId.Value);
        if (!users.Any())
        {
            return [];
        }

        return users
            .Where(x => x.IsConnectedWithExternalIdentity())
            .Select(CreateUserDetails)
            .ToList();
    }

    private static UserDetails CreateUserDetails(ContactDto contact)
    {
        var role = UserRoleMapper.ToDomain(contact.webrole);
        return new UserDetails(UserGlobalId.From(contact.contactExternalId), contact.firstName, contact.lastName, contact.email, contact.jobTitle, role, null);
    }
}
