using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Models;

public class ExtendedSelectListItem : SelectListItem
{
    public ExtendedSelectListItem(string text, string value, bool selected, string? hint = null)
        : base(text, value, selected)
    {
        Hint = hint;
    }

    public string? Hint { get; set; }
}
