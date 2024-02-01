using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using EnumerableExtensions = HE.Investments.Common.WWW.Extensions.EnumerableExtensions;

namespace HE.Investments.Common.WWW.Helpers;

public static class SelectListHelper
{
    public static IEnumerable<ExtendedSelectListItem> FromEnumToExtendedList<TEnum>()
        where TEnum : struct, Enum
    {
        var values = EnumExtensions.GetDefinedValues<TEnum>();
        return EnumerableExtensions.ToSelectList(values);
    }

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

    public static ExtendedSelectListItem FromEnumToExtendedListItem<TEnum>(TEnum value)
        where TEnum : struct, Enum
    {
        return new ExtendedSelectListItem(value.GetDescription(), value.ToString(), false);
    }
}
