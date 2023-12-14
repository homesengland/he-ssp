using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Organisation.Services;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Account.Domain.Organisation.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly IOrganizationServiceAsync2 _organizationServiceAsync;

    private readonly IAccountUserContext _loanUserContext;

    private readonly IContactService _contactService;

    public ContactRepository(
        IOrganizationServiceAsync2 organizationServiceAsync,
        IAccountUserContext loanUserContext,
        IContactService contactService)
    {
        _organizationServiceAsync = organizationServiceAsync;
        _loanUserContext = loanUserContext;
        _contactService = contactService;
    }

    public async Task LinkOrganisation(OrganisationId organisationId, int portalType)
    {
        await _contactService.LinkContactWithOrganization(
           _organizationServiceAsync,
           _loanUserContext.UserGlobalId.ToString(),
           organisationId.Value,
           portalType);
        await _loanUserContext.RefreshAccounts();
    }
}
