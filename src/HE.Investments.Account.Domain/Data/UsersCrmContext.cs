using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Exceptions;
using HE.Investments.Organisation.Services;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Account.Domain.Data;

public class UsersCrmContext : IUsersCrmContext
{
    private readonly IOrganizationServiceAsync2 _organizationServiceAsync;
    private readonly IContactService _contactService;

    public UsersCrmContext(
        IOrganizationServiceAsync2 organizationServiceAsync,
        IContactService contactService)
    {
        _organizationServiceAsync = organizationServiceAsync;
        _contactService = contactService;
    }

    public async Task<IList<ContactDto>> GetUsers(Guid organisationId)
    {
        return await _contactService.GetAllOrganisationContactsForPortal(_organizationServiceAsync, organisationId);
    }

    public async Task<ContactDto> GetUser(string id)
    {
        var user = await _contactService.RetrieveUserProfile(_organizationServiceAsync, id)
            ?? throw new NotFoundException($"Cannot find User with id {id}.");

        return user;
    }

    public async Task<int?> GetUserRole(string id, Guid organisationId)
    {
        var roles = await _contactService.GetContactRolesForOrganisationContacts(
            _organizationServiceAsync,
            new List<string> { id },
            organisationId);

        return roles.Select(GetUserRole).FirstOrDefault();
    }

    public async Task ChangeUserRole(string userId, int role, Guid organisationId)
    {
        await _contactService.UpdateContactWebrole(_organizationServiceAsync, userId, organisationId, role);
    }

    private static int? GetUserRole(ContactRolesDto dto)
    {
        return dto.contactRoles.Select(r => r.permission).FirstOrDefault();
    }
}
