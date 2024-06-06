using System.Collections.Generic;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.AHP.Plugins.Repositories
{
    public interface IGrantBenchmarkRepository : ICrmEntityRepository<invln_grantbenchmark, DataverseContext>
    {
        invln_grantbenchmark GetGrantBenchmark(
            invln_Tenurechoice tenure,
            int governmentOfficeRegionId,
            invln_BenchmarkTable benchmarkTable,
            params string[] columns);

        IEnumerable<invln_grantbenchmark> GetGrantBenchmarks(
            invln_Tenurechoice tenure,
            int governmentOfficeRegionId,
            params string[] columns);
    }
}
