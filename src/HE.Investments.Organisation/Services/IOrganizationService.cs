using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Organisation.Services;

public interface IOrganizationService
{
    Task<OrganizationDetailsDto> GetOrganizationDetails(string accountId, string contactExternalId);

    Guid CreateOrganization(OrganizationDetailsDto organizationDetails);

    Task<Guid> CreateOrganisationChangeRequest(OrganizationDetailsDto organizationDetails, string contactExternalId);

    Task<ContactDto?> GetOrganisationChangeDetailsRequestContact(Guid accountId);
}
