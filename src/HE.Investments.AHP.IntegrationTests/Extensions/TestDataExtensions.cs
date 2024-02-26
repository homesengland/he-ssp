using System.Globalization;

namespace HE.Investments.AHP.IntegrationTests.Extensions;

public static class TestDataExtensions
{
    public static string WithTimestampPrefix(this string text)
    {
        return $"{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}: {text}";
    }
}
