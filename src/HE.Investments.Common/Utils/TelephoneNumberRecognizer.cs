namespace HE.Investments.Common.Utils;

public static class TelephoneNumberRecognizer
{
    public static bool StartWithCountryCode(string telephoneNumber)
    {
        return telephoneNumber.StartsWith("+", StringComparison.InvariantCulture)
               || telephoneNumber.StartsWith("00", StringComparison.InvariantCulture);
    }

    public static bool IsUkCountryCode(string telephoneNumber)
    {
        return telephoneNumber.StartsWith("+44", StringComparison.InvariantCulture)
               || telephoneNumber.StartsWith("0044", StringComparison.InvariantCulture);
    }

    public static string StripToNationalFormat(string telephoneNumber)
    {
        var prefixes = new Dictionary<string, int>
        {
            { "+", 3 },
            { "00", 4 },
            { "(0)", 3 },
            { "0", 1 },
        };

        foreach (var prefix in prefixes
                     .Where(prefix =>
                         telephoneNumber.StartsWith(prefix.Key, StringComparison.InvariantCulture)))
        {
            telephoneNumber = telephoneNumber[prefix.Value..];
        }

        return telephoneNumber;
    }

    public static string StripFromSpecialCharacters(string telephoneNumber)
    {
        telephoneNumber = telephoneNumber.Replace(" ", string.Empty);
        return telephoneNumber.Replace("-", string.Empty);
    }
}
