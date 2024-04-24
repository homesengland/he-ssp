using HE.Investments.Common.WWW.Components;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Models;

public class DynamicSelectListItem : SelectListItem
{
    public DynamicSelectListItem(string value, bool selected, DynamicComponentViewModel item)
        : base(string.Empty, value, selected)
    {
        Item = item;
    }

    public DynamicComponentViewModel Item { get; }
}
