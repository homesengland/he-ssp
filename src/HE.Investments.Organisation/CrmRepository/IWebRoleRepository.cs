using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace HE.Investments.Organisation.CrmRepository;
public interface IWebRoleRepository
{
    List<Entity> GetContactWebrole(IOrganizationServiceAsync2 service, Guid contactId, string portalTypeFilter);

    List<Entity> GetDefaultPortalRoles(IOrganizationServiceAsync2 service, int portalType);

    Entity? GetContactWebroleForGivenOrganisationAndPortal(IOrganizationServiceAsync2 service, Guid organisationId, Guid contactId, string? portalTypeFiler = null);

    Entity? GetContactWebroleForOrganisation(IOrganizationServiceAsync2 service, Guid contactId, Guid organisationId);

    Entity? GetWebroleByName(IOrganizationServiceAsync2 service, string webroleName);

    Entity? GetWebroleByPermissionOptionSetValue(IOrganizationServiceAsync2 service, int permission, string portalTypeFilter);

    List<Entity> GetWebrolesForPassedContacts(IOrganizationServiceAsync2 service, string contactExternalIds, Guid organisationGuid);
}
