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
        return await _contactService.GetAllOrganisationContactsForPortal(
            _organizationServiceAsync,
            await TryGetOrganisationId());
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

    public async Task<int?> GetUserRole(string id)
    {
        var roles = await _contactService.GetContactRolesForOrganisationContacts(
            _organizationServiceAsync,
            new List<string> { id },
            await TryGetOrganisationId());

        return roles.Select(GetUserRole).FirstOrDefault();
    }

    public async Task<Dictionary<string, int?>> GetUsersRole(List<string> userIds)
    {
        var roles = await _contactService.GetContactRolesForOrganisationContacts(
            _organizationServiceAsync,
            userIds,
            await TryGetOrganisationId());

        return roles.ToDictionary(c => c.externalId, GetUserRole);
    }

    public async Task ChangeUserRole(string userId, int role)
    {
        var organisationId = await TryGetOrganisationId();
        await _contactService.UpdateContactWebrole(_organizationServiceAsync, userId, organisationId, role);

        await _accountUserContext.RefreshProfileDetails();
    }

    private static int? GetUserRole(ContactRolesDto dto)
    {
        return dto.contactRoles.Select(r => r.permission).FirstOrDefault();
    }

    private async Task<Guid> TryGetOrganisationId()
    {
        var account = await _accountUserContext.GetSelectedAccount();

        if (account.AccountId == null)
        {
            throw new InvalidOperationException("User is not linked with any organisation.");
        }

        return account.AccountId.Value;
    }
}
