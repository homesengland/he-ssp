using System.Linq;
using DataverseModel;
using HE.Base.Repositories;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.AHP.Plugins.Repositories
{
    public class GrantBenchmarkRepository : CrmEntityRepository<invln_grantbenchmark, DataverseContext>, IGrantBenchmarkRepository
    {
        public GrantBenchmarkRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public invln_grantbenchmark GetRegionalBenchmarkGrantPerUnit(
            invln_Tenurechoice tenure,
            int governmentOfficeRegionId,
            params string[] columns)
        {
            logger.Trace($"FindRegionalBenchmark tenure: {tenure}, governmentOfficeRegionId: {governmentOfficeRegionId}");

            var benchmarkTable = invln_BenchmarkTable.Table5RegionalBenchmarkGrantPerUnit;

            var query = new QueryExpression(invln_grantbenchmark.EntityLogicalName)
            {
                ColumnSet = columns.Any() ? new ColumnSet(columns) : new ColumnSet(true),
                Criteria = new FilterExpression(LogicalOperator.And)
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = invln_grantbenchmark.Fields.invln_tenure,
                            Operator = ConditionOperator.Equal,
                            Values = { (int)tenure }
                        },
                        new ConditionExpression
                        {
                            AttributeName = invln_grantbenchmark.Fields.invln_GovernmentOfficeRegion,
                            Operator = ConditionOperator.Equal,
                            Values = { governmentOfficeRegionId }
                        },
                        new ConditionExpression()
                        {
                            AttributeName = invln_grantbenchmark.Fields.invln_BenchmarkTable,
                            Operator = ConditionOperator.Equal,
                            Values = { (int)benchmarkTable }
                        }
                    }
                }
            };

            var result = RetrieveAll(query).Entities.Select(e => e.ToEntity<invln_grantbenchmark>());
            return result.Single();
        }
    }
}
