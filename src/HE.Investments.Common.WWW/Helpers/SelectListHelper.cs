using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Helpers;

public static class SelectListHelper
{
    public static SelectListItem FromEnum<TEnum>(TEnum value, string label)
        where TEnum : struct
    {
        return new SelectListItem
        {
            Value = value.ToString(),
            Text = label,
        };
    }
}
