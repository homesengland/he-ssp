using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Organisation.Services;
public interface IOrganizationService
{
    Task<OrganizationDetailsDto> GetOrganizationDetails(string accountid, string contactExternalId);

    Guid CreateOrganization(OrganizationDetailsDto organizationDetails);
}
