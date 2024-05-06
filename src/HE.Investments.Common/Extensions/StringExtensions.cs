using System.Globalization;
using HE.Investments.Common.Contract;

namespace HE.Investments.Common.Extensions;

public static class StringExtensions
{
    public static bool IsProvided(this string? val)
    {
        return !string.IsNullOrWhiteSpace(val);
    }

    public static bool IsNotProvided(this string? val)
    {
        return !val.IsProvided();
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

    public static IList<string>? ToOneElementList(this string? val)
    {
        return val != null ? new List<string> { val } : null;
    }

    public static string? NormalizeLineEndings(this string? value)
    {
        return value?
            .Replace("\r\n", "\n")
            .Replace("\r", "\n");
    }

    public static string? WithoutPercentageChar(this string? value) => value?.Replace("%", string.Empty, StringComparison.InvariantCulture);

    public static string TryToGuidAsString(this string value) => ShortGuid.TryToGuidAsString(value);

    public static string ToGuidAsString(this string value) => ShortGuid.ToGuidAsString(value);
}
