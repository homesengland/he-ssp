using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace HE.Investments.Organisation.CrmRepository;
public class OrganisationChangeRequestRepository : IOrganisationChangeRequestRepository
{
    public async Task<Entity?> GetChangeRequestForOrganisation(IOrganizationServiceAsync2 service, string organisationId)
    {
        var condition1 = new ConditionExpression("invln_organisationid", ConditionOperator.Equal, organisationId);
        var filter1 = new FilterExpression()
        {
            Conditions =
                {
                    condition1,
                },
            FilterOperator = LogicalOperator.And,
        };
        var cols = new ColumnSet(true);

        var query = new QueryExpression("invln_organisationchangerequest")
        {
            ColumnSet = cols,
        };
        query.Criteria.FilterOperator = LogicalOperator.And;
        query.Criteria.AddFilter(filter1);

        var result1 = await service.RetrieveMultipleAsync(query);
        return result1.Entities.FirstOrDefault();
    }
}
