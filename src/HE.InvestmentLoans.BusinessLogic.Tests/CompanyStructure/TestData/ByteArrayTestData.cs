namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestData;

public static class ByteArrayTestData
{
    public static readonly byte[] ByteArray1Kb = Enumerable.Repeat<byte>(0x20, 1024).ToArray();

    public static readonly byte[] ByteArray1MbAnd1Kb = Enumerable.Repeat<byte>(0x20, 1024 + 1_048_576).ToArray();
}
