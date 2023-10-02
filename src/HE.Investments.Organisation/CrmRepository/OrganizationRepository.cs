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

    public Entity? GetDefaultAccount(IOrganizationServiceAsync2 service)
    {
        var condition1 = new ConditionExpression("name", ConditionOperator.Equal, "DO_NOT_DELETE_DEFAULT_ACCOUNT");
        var filter1 = new FilterExpression()
        {
            Conditions =
                {
                    condition1,
                },
            FilterOperator = LogicalOperator.Or,
        };
        var cols = new ColumnSet(true);

        var query = new QueryExpression("account")
        {
            ColumnSet = cols,
        };
        query.Criteria.FilterOperator = LogicalOperator.And;
        query.Criteria.AddFilter(filter1);

        var result1 = service.RetrieveMultiple(query);
        return result1.Entities.FirstOrDefault();
    }

    public Entity? GetOrganizationViaCompanyHouseNumber(IOrganizationServiceAsync2 service, string companyHouseNumber)
    {
        var condition1 = new ConditionExpression("he_companieshousenumber", ConditionOperator.Equal, companyHouseNumber);
        var filter1 = new FilterExpression()
        {
            Conditions =
                {
                    condition1,
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
            throw new InvalidPluginExecutionException("Organization with he_companieshousenumber: " + companyHouseNumber + " does not extst in CRM");
        }

        return result1.Entities.FirstOrDefault();
    }

    public EntityCollection? SearchForOrganizationsByName(IOrganizationServiceAsync2 service, IEnumerable<string> names, bool recordsWithoutCopanyNumberIncluded)
    {
        if (names != null)
        {
            var filter1 = new FilterExpression
            {
                FilterOperator = LogicalOperator.And,
            };

            var cols = new ColumnSet("name", "he_companieshousenumber", "address1_line1", "address1_line2", "address1_line3", "address1_city", "address1_postalcode", "address1_country");

            var query = new QueryExpression("account")
            {
                ColumnSet = cols,
            };

            var numberOfRequestsInQuery = 1;
            var recordsWithoutCompanyNumberFilter = new FilterExpression();
            if (!recordsWithoutCopanyNumberIncluded)
            {
                recordsWithoutCompanyNumberFilter = new FilterExpression
                {
                    FilterOperator = LogicalOperator.And,
                    Conditions =
                    {
                        new ConditionExpression("he_companieshousenumber", ConditionOperator.NotNull),
                    },
                };
            }

            var retrievedEntitiesCollection = new EntityCollection();
            EntityCollection retrievedEntities;
            foreach (var name in names)
            {
                var condition1 = new ConditionExpression("name", ConditionOperator.Like, $"%{name}%");
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

                    query.Criteria.AddFilter(recordsWithoutCompanyNumberFilter);

                    filter1.Conditions.Clear();
                    query.Criteria.Filters.Clear();
                }
            }

            query.Criteria.AddFilter(filter1);
            query.Criteria.AddFilter(recordsWithoutCompanyNumberFilter);

            retrievedEntities = service.RetrieveMultiple(query);
            if (retrievedEntities != null)
            {
                retrievedEntitiesCollection.Entities.AddRange(retrievedEntities.Entities);
            }

            return retrievedEntitiesCollection;
        }

        return null;
    }

    public EntityCollection? SearchForOrganizationsByCompanyHouseNumber(IOrganizationServiceAsync2 service, IEnumerable<string> organizationNumbers)
    {
        if (organizationNumbers != null)
        {
            var filter1 = new FilterExpression
            {
                FilterOperator = LogicalOperator.And,
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
