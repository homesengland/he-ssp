using System.Security.Cryptography.X509Certificates;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared;
using HE.Investments.Loans.Common.Exceptions;
using HE.Investments.Organisation.Services;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Account.Domain.Data;

public class UsersCrmContext : IUsersCrmContext
{
    private readonly IOrganizationServiceAsync2 _organizationServiceAsync;
    private readonly IAccountUserContext _accountUserContext;
    private readonly IContactService _contactService;

    public UsersCrmContext(
        IOrganizationServiceAsync2 organizationServiceAsync,
        IAccountUserContext accountUserContext,
        IContactService contactService)
    {
        _organizationServiceAsync = organizationServiceAsync;
        _accountUserContext = accountUserContext;
        _contactService = contactService;
    }

    public async Task<IList<ContactDto>> GetUsers()
    {
        var account = await _accountUserContext.GetSelectedAccount();

        if (account.AccountId == null)
        {
            throw new InvalidOperationException("Cannot fetch users for missing organisation.");
        }

        return await _contactService.GetAllOrganisationContactsForPortal(
            _organizationServiceAsync,
            account.AccountId.ToString()!);
    }

    public async Task<ContactDto> GetUser(string id)
    {
        var user = await _contactService.RetrieveUserProfile(_organizationServiceAsync, id);
        if (user == null)
        {
            throw new NotFoundException($"Cannot find User with id {id}.");
        }

        return user;
    }

    public async Task<string?> GetUserRole(string id, string email)
    {
        // TODO #86130: remove portal type after CRM changes
        var roles = await _contactService.GetContactRoles(_organizationServiceAsync, email, PortalConstants.AhpPortalType, id);

        return roles?.contactRoles.Select(r => r.webRoleName).FirstOrDefault();
    }

    public async Task ChangeUserRole(string userId, string role)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        if (account.AccountId == null)
        {
            throw new InvalidOperationException("Cannot assign role for user without linked organisation.");
        }

        await _contactService.UpdateContactWebrole(_organizationServiceAsync, userId, account.AccountId.Value, role);
    }
}
