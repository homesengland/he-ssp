using HE.Investments.Common.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Helpers;

public static class SelectListHelper
{
    public static IEnumerable<SelectListItem> FromEnum<TEnum>()
        where TEnum : struct, Enum
    {
        return EnumExtensions.GetDefinedValues<TEnum>().Select(FromEnum);
    }

    public static SelectListItem FromEnum<TEnum>(TEnum value)
        where TEnum : struct, Enum
    {
        return FromEnum(value, value.GetDescription());
    }

    public static SelectListItem FromEnum<TEnum>(TEnum value, string label)
        where TEnum : struct, Enum
    {
        return new SelectListItem
        {
            Value = value.ToString(),
            Text = label,
        };
    }
}
