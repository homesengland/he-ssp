using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace HE.Investments.Organisation.CrmRepository;
public interface IWebRoleRepository
{
    List<Entity> GetContactWebRole(IOrganizationServiceAsync2 service, string contactId, string portalTypeFilter);

    List<Entity> GetDefaultPortalRoles(IOrganizationServiceAsync2 service, int portalType);

    Entity? GetContactWebRoleForGivenOrganisationAndPortal(IOrganizationServiceAsync2 service, string organisationId, string contactId, string? portalTypeFiler = null);

    Entity? GetContactWebRoleForOrganisation(IOrganizationServiceAsync2 service, string contactId, string organisationId);

    Entity? GetWebRoleByName(IOrganizationServiceAsync2 service, string webRoleName);

    Entity? GetWebRoleByPermissionOptionSetValue(IOrganizationServiceAsync2 service, int permission, string portalTypeFilter);

    List<Entity> GetWebRolesForPassedContacts(IOrganizationServiceAsync2 service, string contactExternalIds, string organisationId);
}
