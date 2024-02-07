using System.Globalization;

namespace HE.Investments.Common.Extensions;

public static class DecimalExtensions
{
    public static decimal ToWholeNumberRoundFloor(this decimal val)
    {
        return Convert.ToInt64(Math.Floor(val), CultureInfo.InvariantCulture);
    }

    public static decimal RoundToTwoDecimalPlaces(this decimal val)
    {
        return Math.Round(val, 2, MidpointRounding.AwayFromZero);
    }

    public static string? ToPercentage100(this decimal? value) => value?.ToString("0.##\\%", CultureInfo.InvariantCulture);

    public static string? ToWholePercentage(this decimal? value) => value?.ToString("0%", CultureInfo.InvariantCulture);

    public static string? ToWholePercentage(this decimal value) => ((decimal?)value).ToWholePercentage();
}
