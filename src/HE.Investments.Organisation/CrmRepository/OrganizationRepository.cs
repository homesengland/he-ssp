using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Organisation.CrmRepository;
public class OrganizationRepository : IOrganizationRepository
{
    public Guid? EnsureCreateOrganization(IOrganizationServiceAsync2 service, string companyNumber, string companyName)
    {
        var condition1 = new ConditionExpression("he_companieshousenumber", ConditionOperator.Equal, companyNumber);

        FilterExpression filter1 = new FilterExpression()
        {
            Conditions =
                {
                    condition1,
                },
            FilterOperator = LogicalOperator.Or
        };
        if (!String.IsNullOrEmpty(companyName))
        {
            ConditionExpression condition2 = new ConditionExpression("name", ConditionOperator.Like, $"%{companyName}%");
            filter1.Conditions.Add(condition2);
        }

        var cols = new ColumnSet("name");

        var query = new QueryExpression("account");
        query.ColumnSet = cols;
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
