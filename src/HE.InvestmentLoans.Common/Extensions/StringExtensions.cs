using System.Globalization;

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

    public static string TitleCaseFirstLetterInString(this string val)
    {
        var result = string.Empty;
        var words = val.Split(' ');

        if (words.Length > 0)
        {
            words[0] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[0]);
            result = string.Join(" ", words);
        }

        return result;
    }
}
