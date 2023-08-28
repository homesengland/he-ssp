using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace HE.Investments.Organisation.CrmRepository;
public class ContactRepository : IContactRepository
{
    public Entity? GetContactViaExternalId(IOrganizationServiceAsync2 service, string contactExternalId, string[]? columnSet = null)
    {
        var keys = new KeyAttributeCollection
                {
                    { "invln_externalid", contactExternalId },
                };

        var request = new RetrieveRequest
        {
            ColumnSet = new ColumnSet(columnSet),
            Target = new EntityReference("contact", keys),
        };

        try
        {
            var response = (RetrieveResponse)service.Execute(request);
            if (response != null)
            {
                return response.Entity;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            throw new InvalidPluginExecutionException("Contact with invln_externalid: " + contactExternalId + " does not extst in CRM");
        }
    }
}
