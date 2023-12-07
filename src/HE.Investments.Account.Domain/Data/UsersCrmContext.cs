using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared;
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

    public async Task<IList<ContactDto>> GetUsers(string portalType)
    {
        var account = await _accountUserContext.GetSelectedAccount();

        if (account.AccountId == null)
        {
            throw new InvalidOperationException("Cannot fetch users for missing organisation.");
        }

        return await _contactService.GetAllOrganisationContactsForPortal(
            _organizationServiceAsync,
            account.AccountId.ToString()!,
            portalType);
    }
}
