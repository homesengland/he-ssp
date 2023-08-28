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
                    condition1, condition2,
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

    EntityCollection? IOrganizationRepository.SearchForOrganizations(IOrganizationServiceAsync2 service, List<string> organizationNumbers)
    {
        if (organizationNumbers != null)
        {
            var filter1 = new FilterExpression
            {
                FilterOperator = LogicalOperator.Or
            };

            var cols = new ColumnSet("name", "he_companieshousenumber", "address1_line1", "address1_line2", "address1_line3", "address1_city", "address1_postalcode", "address1_country");

            var query = new QueryExpression("account")
            {
                ColumnSet = cols,
            };

            var numberOfRequestsInQuery = 1;

            var retrievedEntitiesCollection = new EntityCollection();
            EntityCollection retrievedEntities;
            foreach (var organizationNumber in organizationNumbers)
            {
                var condition1 = new ConditionExpression("he_companieshousenumber", ConditionOperator.Equal, organizationNumber);
                filter1.Conditions.Add(condition1);
                numberOfRequestsInQuery++;
                if (numberOfRequestsInQuery >= 490)
                {
                    numberOfRequestsInQuery = 0;
                    query.Criteria.AddFilter(filter1);

                    retrievedEntities = service.RetrieveMultiple(query);
                    if (retrievedEntities != null)
                    {
                        retrievedEntitiesCollection.Entities.AddRange(retrievedEntities.Entities);
                    }

                    filter1.Conditions.Clear();
                    query.Criteria.Filters.Clear();
                }
            }

            query.Criteria.AddFilter(filter1);

            retrievedEntities = service.RetrieveMultiple(query);
            if (retrievedEntities != null)
            {
                retrievedEntitiesCollection.Entities.AddRange(retrievedEntities.Entities);
            }

            return retrievedEntitiesCollection;
        }

        return null;
    }
}
