using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Organisation.CrmRepository;
public interface IWebRoleRepository
{
    List<Entity> GetContactWebrole(IOrganizationServiceAsync2 service, Guid contactId, string portalType);

    List<Entity> GetDefaultPortalRoles(IOrganizationServiceAsync2 service, string portalType);
}
