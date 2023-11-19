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

        return Convert.ToInt64(Math.Truncate(val.Value), CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
    }
}
