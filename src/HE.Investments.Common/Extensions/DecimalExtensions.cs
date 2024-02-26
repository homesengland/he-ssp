using System.Globalization;

namespace HE.Investments.Common.Extensions;

public static class DecimalExtensions
{
    public static decimal ToWholeNumberRoundFloor(this decimal val)
    {
        return Convert.ToInt64(Math.Floor(val), CultureInfo.InvariantCulture);
    }

    public static decimal? ToWholeNumberRoundFloor(this decimal? val)
    {
        if (val == null)
        {
            return null;
        }

        return Convert.ToInt64(Math.Floor(val.Value), CultureInfo.InvariantCulture);
    }

    public static decimal RoundToTwoDecimalPlaces(this decimal value)
    {
        return ((decimal?)value).RoundToDecimalPlaces()!.Value;
    }

    public static decimal? RoundToTwoDecimalPlaces(this decimal? value)
    {
        return value.RoundToDecimalPlaces();
    }

    public static decimal RoundToFourDecimalPlaces(this decimal value)
    {
        return ((decimal?)value).RoundToDecimalPlaces(4)!.Value;
    }

    public static decimal? RoundToFourDecimalPlaces(this decimal? value)
    {
        return value.RoundToDecimalPlaces(4);
    }

    public static decimal? RoundToDecimalPlaces(this decimal? value, int precision = 2)
    {
        if (value == null)
        {
            return null;
        }

        var step = (decimal)Math.Pow(10, precision);
        var tmp = Math.Truncate(step * value.Value);
        return tmp / step;
    }

    public static int ToPercentage100(this decimal value) => (int)(value * 100);

    public static string? ToPercentage100(this decimal? value) => value?.ToString("0.##\\%", CultureInfo.InvariantCulture);

    public static string? ToWholePercentage(this decimal? value) => value?.ToString("0%", CultureInfo.InvariantCulture);

    public static string? ToWholePercentage(this decimal value) => ((decimal?)value).ToWholePercentage();

    public static string? ToPercentageWithTwoDecimal(this decimal? value) => (value * 100m)?.ToString("0.00\\%", CultureInfo.InvariantCulture);
}
