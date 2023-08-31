using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Organisation.CrmRepository;
public interface IPortalPermissionRepository
{
    List<Entity> RetrieveAll(IOrganizationServiceAsync2 service);
}
