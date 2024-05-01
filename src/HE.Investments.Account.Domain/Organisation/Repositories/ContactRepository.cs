using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Organisation.Services;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Account.Domain.Organisation.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly IOrganizationServiceAsync2 _organizationServiceAsync;

    private readonly IAccountUserContext _userContext;

    private readonly IContactService _contactService;

    public ContactRepository(
        IOrganizationServiceAsync2 organizationServiceAsync,
        IAccountUserContext userContext,
        IContactService contactService)
    {
        _organizationServiceAsync = organizationServiceAsync;
        _userContext = userContext;
        _contactService = contactService;
    }

    public async Task LinkOrganisation(OrganisationId organisationId, int portalType)
    {
        await _contactService.LinkContactWithOrganization(
           _organizationServiceAsync,
           _userContext.UserGlobalId.ToString(),
           organisationId.Value,
           portalType);
    }
}
