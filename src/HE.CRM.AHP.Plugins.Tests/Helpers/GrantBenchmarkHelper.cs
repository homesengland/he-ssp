using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Tests.Helpers
{

    public static class GrantBenchmarkHelper
    {
        public static invln_grantbenchmark CreateGrantBenchmark(he_GovernmentOfficeRegion governmentOfficeRegion, invln_Tenurechoice tenure, invln_BenchmarkTable benchmarkTable, invln_BenchmarkValueType valueType, decimal value)
        {
            var grantBenchmark = new invln_grantbenchmark()
            {
                Id = Guid.NewGuid(),
                invln_GovernmentOfficeRegion = new OptionSetValue((int)governmentOfficeRegion),
                invln_tenure = new OptionSetValue((int)tenure),
                invln_BenchmarkTable = new OptionSetValue((int)benchmarkTable),
                invln_BenchmarkValueType = new OptionSetValue((int)valueType)
            };

            if (valueType == invln_BenchmarkValueType.Pounds)
            {
                grantBenchmark.invln_benchmarkgpu = new Money(value);
                grantBenchmark.invln_BenchmarkValuePercentage = null;
            }
            else
            {
                grantBenchmark.invln_BenchmarkValuePercentage = value;
                grantBenchmark.invln_benchmarkgpu = null;
            }

            return grantBenchmark;
        }

        public static invln_grantbenchmark CreateGrantBenchmarkTable1(he_GovernmentOfficeRegion governmentOfficeRegion, invln_Tenurechoice tenure, decimal benchmarkValuePercentage)
        {
            return CreateGrantBenchmark(governmentOfficeRegion, tenure, invln_BenchmarkTable.Table1AreaaveragesforGrantasofTotalSchemeCosts, invln_BenchmarkValueType.Percentage, benchmarkValuePercentage);
        }

        public static invln_grantbenchmark CreateGrantBenchmarkTable2(he_GovernmentOfficeRegion governmentOfficeRegion, invln_Tenurechoice tenure, decimal benchmarkValuePercentage)
        {
            return CreateGrantBenchmark(governmentOfficeRegion, tenure, invln_BenchmarkTable.Table2Areaaveragesforworkscostsperm2, invln_BenchmarkValueType.Pounds, benchmarkValuePercentage);
        }

        public static invln_grantbenchmark CreateGrantBenchmarkTable3(he_GovernmentOfficeRegion governmentOfficeRegion, invln_Tenurechoice tenure, decimal benchmarkValuePercentage)
        {
            return CreateGrantBenchmark(governmentOfficeRegion, tenure, invln_BenchmarkTable.Table3Areaaveragesforruralgrantperunit, invln_BenchmarkValueType.Pounds, benchmarkValuePercentage);
        }

        public static invln_grantbenchmark CreateGrantBenchmarkTable4(he_GovernmentOfficeRegion governmentOfficeRegion, invln_Tenurechoice tenure, decimal benchmarkValuePercentage)
        {
            return CreateGrantBenchmark(governmentOfficeRegion, tenure, invln_BenchmarkTable.Table4Areaaveragesforsupportedhousinggrantperunit, invln_BenchmarkValueType.Pounds, benchmarkValuePercentage);
        }

        public static invln_grantbenchmark CreateGrantBenchmarkTable5(he_GovernmentOfficeRegion governmentOfficeRegion, invln_Tenurechoice tenure, decimal benchmarkgpu)
        {
            return CreateGrantBenchmark(governmentOfficeRegion, tenure, invln_BenchmarkTable.Table5RegionalBenchmarkGrantPerUnit, invln_BenchmarkValueType.Pounds, benchmarkgpu);
        }
    }
}
