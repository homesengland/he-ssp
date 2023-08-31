using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace HE.Investments.Organisation.CrmRepository;
public class PortalPermissionRepository : IPortalPermissionRepository
{
    public List<Entity> RetrieveAll(IOrganizationServiceAsync2 service)
    {
        var query = new QueryExpression("invln_portalpermissionlevel")
        {
            ColumnSet = new ColumnSet(allColumns: true),
        };
        var retrievedQueries = service.RetrieveMultiple(query);
        return retrievedQueries.Entities.ToList();
    }
}
