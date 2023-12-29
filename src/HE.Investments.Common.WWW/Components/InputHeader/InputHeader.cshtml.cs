using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Components.InputHeader;

public class InputHeader : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        string? label = null,
        bool? isHeadingLabel = null,
        string? hint = null)
    {
        // We may need to display label as a page heading when there is only one input on the page
        // Is important to display vertical red line next to label/heading in case of error
        return View("InputHeader", (fieldName, label, isHeadingLabel ?? false, hint));
    }
}
