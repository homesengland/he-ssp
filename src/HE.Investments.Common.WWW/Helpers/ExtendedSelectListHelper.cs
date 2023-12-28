using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Models;

namespace HE.Investments.Common.WWW.Helpers;

public static class ExtendedSelectListHelper
{
    public static IEnumerable<ExtendedSelectListItem> FromEnum<TEnum>()
        where TEnum : struct, Enum
    {
        return EnumExtensions.GetDefinedValues<TEnum>().Select(FromEnum);
    }

    public static ExtendedSelectListItem FromEnum<TEnum>(TEnum value)
        where TEnum : struct, Enum
    {
        return FromEnum(value, value.GetDescription());
    }

    public static ExtendedSelectListItem FromEnum<TEnum>(TEnum value, string label)
        where TEnum : struct, Enum
    {
        return new ExtendedSelectListItem(label, value.ToString(), false);
    }
}
