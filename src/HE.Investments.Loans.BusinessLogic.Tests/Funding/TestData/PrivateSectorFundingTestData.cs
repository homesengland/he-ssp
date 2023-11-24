using HE.Investments.Loans.Contract.Funding.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.TestData;

public static class PrivateSectorFundingTestData
{
    public static readonly PrivateSectorFunding PrivateSectorFundingFalse = new(false, string.Empty, "additional not applying reason");
}
