using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.Extensions;

internal static class TagConstructExtensions
{
    public static void ConstructClass(TagHelperOutput output, string cssClass, bool condition = true)
    {
        if (condition)
        {
            TagConstruct.ConstructClass(output, cssClass);
        }
    }
}
