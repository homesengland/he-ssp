using System.Text.RegularExpressions;
using HE.Investments.Common.Extensions;

namespace HE.Investments.Common.WWW.Extensions;

public static class StringExtensions
{
    public static string ToIdTag(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        return value.ToLowerInvariant().Replace(' ', '-');
    }

    public static string ToPascalCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return string.Join(
            string.Empty,
            value.Split(' ')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => x.TitleCaseFirstLetterInString()));
    }
}
