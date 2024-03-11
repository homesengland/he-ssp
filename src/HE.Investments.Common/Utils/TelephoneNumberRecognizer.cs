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
        telephoneNumber = telephoneNumber.Replace(" ", string.Empty);

        if (telephoneNumber.StartsWith("+", StringComparison.InvariantCulture))
        {
            telephoneNumber = telephoneNumber[3..];
        }

        if (telephoneNumber.StartsWith("00", StringComparison.InvariantCulture))
        {
            telephoneNumber = telephoneNumber[4..];
        }

        return telephoneNumber.TrimStart('(', '0', ')');
    }
}
