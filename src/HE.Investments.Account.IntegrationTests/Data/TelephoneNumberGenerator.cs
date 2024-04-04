using System.Globalization;

namespace HE.Investments.Account.IntegrationTests.Data;

public static class TelephoneNumberGenerator
{
    private static readonly Random Random = new();

    public static string GenerateWithCountryCode()
    {
        return $"+44 {GenerateStringNumber(3, false)} {GenerateStringNumber(3)} {GenerateStringNumber(4)}";
    }

    public static string GenerateWithoutCountryCode()
    {
        return $"0{GenerateStringNumber(4, false)} {GenerateStringNumber(3)} {GenerateStringNumber(3)}";
    }

    private static string GenerateStringNumber(int length, bool allowLeadingZero = true)
    {
        return string.Concat(GenerateNumber(length, allowLeadingZero));
    }

    private static IEnumerable<string> GenerateNumber(int length, bool allowLeadingZero = true)
    {
        for (var i = 0; i < length; i++)
        {
            var min = i > 0 || allowLeadingZero ? 0 : 1;
            yield return Random.Next(min, 9).ToString(CultureInfo.InvariantCulture);
        }
    }
}
