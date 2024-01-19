using System.Globalization;

namespace HE.Investments.Common.Extensions;

public static class DecimalExtensions
{
    public static decimal ToWholeNumberRoundFloor(this decimal val)
    {
        return Convert.ToInt64(Math.Floor(val), CultureInfo.InvariantCulture);
    }
}
