namespace HE.InvestmentLoans.Common.Extensions;

public static class StringExtensions
{
    public static int? TryParseNullableInt(this string val)
    {
        return int.TryParse(val, out var outValue) ? outValue : null;
    }

    public static bool IsProvided(this string? val)
    {
        return !string.IsNullOrWhiteSpace(val);
    }
}
