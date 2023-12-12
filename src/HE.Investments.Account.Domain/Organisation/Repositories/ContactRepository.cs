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

    public async Task LinkOrganisation(string organisationId, string portalType)
    {
        // TODO #65730: create correct parameters
        // await _contactService.LinkContactWithOrganization(
        //    _organizationServiceAsync,
        //    _loanUserContext.UserGlobalId.ToString(),
        //    organisationId,
        //    portalType);
        await _loanUserContext.RefreshProfileDetails();
    }
}
