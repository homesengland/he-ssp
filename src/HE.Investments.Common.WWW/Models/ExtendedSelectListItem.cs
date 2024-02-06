using HE.Investments.Common.WWW.Components;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Models;

public class ExtendedSelectListItem : SelectListItem
{
    public ExtendedSelectListItem(string text, string value, bool selected, string? hint = null, DynamicComponentViewModel? expandableChild = null)
        : base(text, value, selected)
    {
        Hint = hint;
        ExpandableChild = expandableChild;
    }

    public string? Hint { get; set; }

    public DynamicComponentViewModel? ExpandableChild { get; }
}
