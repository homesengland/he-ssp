extern alias Org;

using HE.Investments.Account.Shared;
using Microsoft.PowerPlatform.Dataverse.Client;
using Org::HE.Investments.Organisation.Services;

namespace HE.InvestmentLoans.BusinessLogic.Organization.Repositories;

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
        await _contactService.LinkContactWithOrganization(
            _organizationServiceAsync,
            _loanUserContext.UserGlobalId.ToString(),
            organisationId,
            portalType);

        await _loanUserContext.RefreshProfileDetails();
    }
}
