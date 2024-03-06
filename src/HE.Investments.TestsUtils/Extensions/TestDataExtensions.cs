using System.Globalization;

namespace HE.Investments.TestsUtils.Extensions;

public static class TestDataExtensions
{
    public static string WithTimestampPrefix(this string text)
    {
        return $"{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}: {text}";
    }

    public static string WithTimestampSuffix(this string text)
    {
        return $"{text}-{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}";
    }
}
