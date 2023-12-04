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

    public static string ToWholeNumberString(this decimal val)
    {
        return Convert.ToInt64(Math.Truncate(val), CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
    }

    public static string? ToMoneyString(this decimal? val)
    {
        if (val == null)
        {
            return null;
        }

        var exactString = val.Value.ToString("0.00", CultureInfo.InvariantCulture);

        return exactString;
    }

    public static string ToMoneyString(this decimal val)
    {
        return ToMoneyString((decimal?)val) ?? string.Empty;
    }
}
