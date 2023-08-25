using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace HE.Investments.Organisation.CrmRepository;
public class OrganizationRepository : IOrganizationRepository
{
    public Guid? EnsureCreateOrganization(IOrganizationServiceAsync2 service, string companyNumber, string companyName)
    {
        var condition1 = new ConditionExpression("he_companieshousenumber", ConditionOperator.Equal, companyNumber);
        var condition2 = new ConditionExpression("name", ConditionOperator.Equal, companyName);
        var filter1 = new FilterExpression()
        {
            Conditions =
                {
                    condition1, condition2
                },
            FilterOperator = LogicalOperator.Or,
        };
        
        var cols = new ColumnSet("name");

        var query = new QueryExpression("account")
        {
            ColumnSet = cols,
        };
        query.Criteria.FilterOperator = LogicalOperator.And;
        query.Criteria.AddFilter(filter1);

        var result1 = service.RetrieveMultiple(query);
        if (result1.Entities.Count == 0)
        {
            var accountToCreate = new Entity("account");
            accountToCreate["he_companieshousenumber"] = companyNumber;
            accountToCreate["name"] = companyName;
            return service.Create(accountToCreate);
        }

        return null;
    }
}
