using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;

namespace HE.Investments.Common.Extensions;

public static class StringExtensions
{
    public static int? TryParseNullableInt(this string? val)
    {
        return int.TryParse(val, out var outValue) ? outValue : null;
    }

    public static decimal? TryParseNullableDecimal(this string? val)
    {
        return decimal.TryParse(val, out var outValue) ? outValue : null;
    }

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

    public static int? TryParseNullableIntAndValidate(this string? value, string fieldName, string errorMsg, bool allowNull, int minValue, int maxValue, OperationResult operationResult)
    {
        if (string.IsNullOrWhiteSpace(value) && allowNull)
        {
            return null;
        }

        return NumericValidator
            .For(value, fieldName, fieldName, operationResult)
            .IsWholeNumber(errorMsg)
            .IsBetween(minValue, maxValue, errorMsg)
            .IsConditionallyRequired(!allowNull);
    }

    public static string? NormalizeLineEndings(this string? value)
    {
        return value?
            .Replace("\r\n", "\n")
            .Replace("\r", "\n");
    }

    public static string? WithoutPercentageChar(this string? value) => value?.Replace("%", string.Empty, StringComparison.InvariantCulture);
}
