using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Organisation.Services;
public interface IOrganizationService
{
    Task<OrganizationDetailsDto> GetOrganizationDetails(string accountId, string contactExternalId);

    Guid CreateOrganization(OrganizationDetailsDto organizationDetails);

    Task<Guid> CreateOrganisationChangeRequest(OrganizationDetailsDto organizationDetails, Guid contactId);

    Task<ContactDto?> GetOrganisationChangeDetailsRequestContact(Guid accountId);
}
