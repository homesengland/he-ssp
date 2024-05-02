using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Organisation.CrmRepository;
public interface IOrganisationChangeRequestRepository
{
    Task<Entity?> GetChangeRequestForOrganisation(IOrganizationServiceAsync2 service, string organisationId);
}
