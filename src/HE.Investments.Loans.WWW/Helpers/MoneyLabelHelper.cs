namespace HE.Investments.Loans.WWW.Helpers;

public static class MoneyLabelHelper
{
    public static string Pounds(int? amount) => amount == null ? null : $"£{amount}";

    public static string Pounds(decimal? amount) => amount == null ? null : $"£{amount}";

    public static string Pounds(string amount) => amount == null ? null : $"£{amount}";
}
