using HE.InvestmentLoans.Contract.Funding.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.TestData;

public static class PrivateSectorFundingTestData
{
    public static readonly PrivateSectorFunding PrivateSectorFundingFalse = new(false, string.Empty, "additional not applying reason");
}
