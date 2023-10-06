namespace HE.InvestmentLoans.Common.Tests.TestData;

public static class DateTimeTestData
{
    public static readonly DateTime SeptemberDay20Year2023At0736 = new(2023, 9, 20, 7, 36, 0);

    public static readonly DateTime OctoberDay05Year2023At0858 = new(2023, 10, 5, 8, 58, 0);

    public static readonly (string Year, string Month, string Day) IncorrectDateAsStrings = ("2022", "1", "32");

    public static readonly (string Year, string Month, string Day) CorrectDateAsStrings = ("2022", "1", "31");

    public static readonly string CorrectDateDisplay = "31/01/2022";
}
