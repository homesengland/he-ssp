using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Funding.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.TestData;

public static class RepaymentSystemTestData
{
    public static readonly RepaymentSystem RepaymentSystemRefinance = new(new Refinance(FundingFormOption.Refinance, "Additional refinance information"), null);
}
