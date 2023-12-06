using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace HE.Investments.Organisation.CrmRepository;
public class ContactRepository : IContactRepository
{
    public List<Entity> GetContactsForOrganisation(IOrganizationServiceAsync2 service, Guid organisationId, string? portalTypeFilter = null)
    {
        var fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name=""contact"">
                        <attribute name=""emailaddress1"" />
                        <attribute name=""firstname"" />
                        <attribute name=""jobtitle"" />
                        <attribute name=""lastname"" />
                        <link-entity name=""invln_contactwebrole"" from=""invln_contactid"" to=""contactid"">
                          <filter>
                            <condition attribute=""invln_accountid"" operator=""eq"" value=""" + organisationId + @""" />
                          </filter>
                          <link-entity name=""invln_webrole"" from=""invln_webroleid"" to=""invln_webroleid"">
                            <link-entity name=""invln_portal"" from=""invln_portalid"" to=""invln_portalid"">" +
                              portalTypeFilter
                              + @"</link-entity>
                          </link-entity>
                        </link-entity>
                      </entity>
                    </fetch>";

        var result = service.RetrieveMultiple(new FetchExpression(fetchXml));
        return result.Entities.ToList();
    }

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
        catch (Exception)
        {
            throw new InvalidPluginExecutionException("Contact with invln_externalid: " + contactExternalId + " does not extst in CRM");
        }
    }

    public Entity? GetContactWithGivenEmailAndExternalId(IOrganizationServiceAsync2 service, string contactEmail, string contactExternalId)
    {
        var condition1 = new ConditionExpression("invln_externalid", ConditionOperator.Equal, contactExternalId);
        var filter1 = new FilterExpression()
        {
            Conditions =
                {
                    condition1,
                },
            FilterOperator = LogicalOperator.And,
        };
        var cols = new ColumnSet(true);

        var query = new QueryExpression("contact")
        {
            ColumnSet = cols,
        };
        query.Criteria.FilterOperator = LogicalOperator.And;
        query.Criteria.AddFilter(filter1);

        var result1 = service.RetrieveMultiple(query);
        if (result1.Entities.Count == 0)
        {
            var contactToCreate = new Entity("contact");
            contactToCreate["emailaddress1"] = contactEmail;
            contactToCreate["invln_externalid"] = contactExternalId;
            contactToCreate.Id = service.Create(contactToCreate);
            return contactToCreate;
        }
        else
        {
            return result1.Entities.FirstOrDefault();
        }
    }
}
