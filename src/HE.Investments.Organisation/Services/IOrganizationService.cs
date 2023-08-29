using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Organisation.Services;
public interface IOrganizationService
{
    OrganizationDetailsDto GetOrganizationDetails(IOrganizationServiceAsync2 service, string accountid, string contactExternalId);
}
