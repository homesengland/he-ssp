using HE.Investments.Loans.Contract.Common;

namespace HE.Investments.Loans.Common.Tests.TestData;
public static class PoundsTestData
{
    public const string CorrectAmountAsString = "9.9";

    public const string IncorrectAmountAsString = "asd";

    public const string CorrectAmountDisplay = "£9.9";

    public const decimal CorrectAmount = 9.9M;

    public static readonly Pounds AnyAmount = new(CorrectAmount);
}
