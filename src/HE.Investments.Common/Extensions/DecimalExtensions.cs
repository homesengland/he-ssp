using System.Globalization;

namespace HE.Investments.Common.Extensions;

public static class DecimalExtensions
{
    public static string? ToWholeNumberString(this decimal? val)
    {
        if (val == null)
        {
            return null;
        }

        return val.Value.ToWholeNumberString();
    }

    public static decimal ToWholeNumber(this decimal val)
    {
        return Convert.ToInt64(Math.Truncate(val), CultureInfo.InvariantCulture);
    }

    public static decimal ToWholeNumberRoundFloor(this decimal val)
    {
        return Convert.ToInt64(Math.Floor(val), CultureInfo.InvariantCulture);
    }

    public static string ToWholeNumberString(this decimal val)
    {
        return val.ToString(CultureInfo.InvariantCulture);
    }

    public static string? ToPoundsPencesString(this decimal? val)
    {
        return val?.ToPoundsPencesString();
    }

    public static string? ToPoundsPencesString(this decimal val)
    {
        var exactString = val.ToString("0.00", CultureInfo.InvariantCulture).Replace(".00", string.Empty);
        return exactString;
    }
}
