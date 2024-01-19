using System.Globalization;
using FluentAssertions.Primitives;

namespace HE.Investments.TestsUtils.Extensions;

public static class CurrencyFluentAssertions
{
    public static void BePoundsOnly(this StringAssertions stringAssertions, decimal expectedValue)
    {
        var expected = $"£{expectedValue.ToString("N0", CultureInfo.GetCultureInfo("en-GB"))}";
        stringAssertions.Be(expected, "Pounds only in UK format");
    }

    public static void BePoundsPences(this StringAssertions stringAssertions, decimal expectedValue)
    {
        var expected = $"£{expectedValue.ToString("N2", CultureInfo.GetCultureInfo("en-GB"))}"
            .Replace(".00", string.Empty);
        stringAssertions.Be(expected, "Pounds and pences without zero pences in UK format");
    }
}
