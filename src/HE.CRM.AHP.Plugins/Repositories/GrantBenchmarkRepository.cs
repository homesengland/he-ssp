using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Base.Repositories;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.AHP.Plugins.Repositories
{
    public class GrantBenchmarkRepository : CrmEntityRepository<invln_grantbenchmark, DataverseContext>, IGrantBenchmarkRepository
    {
        public static readonly string[] DefaultColumns = new string[]
        {
            invln_grantbenchmark.Fields.invln_grantbenchmarkname,
            invln_grantbenchmark.Fields.invln_tenure,
            invln_grantbenchmark.Fields.invln_GovernmentOfficeRegion,
            invln_grantbenchmark.Fields.invln_BenchmarkTable,
            invln_grantbenchmark.Fields.invln_BenchmarkValueType,
            invln_grantbenchmark.Fields.invln_benchmarkgpu,
            invln_grantbenchmark.Fields.invln_BenchmarkValuePercentage
        };

        public GrantBenchmarkRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public invln_grantbenchmark GetGrantBenchmark(
            invln_Tenurechoice tenure,
            int governmentOfficeRegionId,
            invln_BenchmarkTable benchmarkTable,
            params string[] columns)
        {
            logger.Trace($"GetGrantBenchmark tenure: {tenure}, governmentOfficeRegionId: {governmentOfficeRegionId}, benchmarkTable: {benchmarkTable}");

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

        public IEnumerable<invln_grantbenchmark> GetGrantBenchmarks(
            invln_Tenurechoice tenure,
            int governmentOfficeRegionId,
            params string[] columns)
        {
            logger.Trace($"GetGrantBenchmarks tenure: {tenure}, governmentOfficeRegionId: {governmentOfficeRegionId}");

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
                        }
                    }
                }
            };

            return RetrieveAll(query).Entities.Select(e => e.ToEntity<invln_grantbenchmark>());
        }
    }
}
