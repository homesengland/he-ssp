using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.Enums;

namespace HE.InvestmentLoans.Common.Tests.TestData;
public static class SourceOfValuationTestData
{
    public const string AnySourceAsString = "ricsRedBookValuation";

    public const string AnySourceDisplay = "RICS Red Book valuation";

    public const SourceOfValuation AnySource = SourceOfValuation.RicsRedBookValuation;
}
