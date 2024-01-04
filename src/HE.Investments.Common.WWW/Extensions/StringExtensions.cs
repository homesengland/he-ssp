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
}
