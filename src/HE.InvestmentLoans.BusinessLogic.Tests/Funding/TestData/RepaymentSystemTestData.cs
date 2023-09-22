using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.TestData;

public static class RepaymentSystemTestData
{
    public static readonly RepaymentSystem RepaymentSystemRefinance = new(new Refinance(FundingFormOption.Refinance, "Additional refinance information"), null);
}
