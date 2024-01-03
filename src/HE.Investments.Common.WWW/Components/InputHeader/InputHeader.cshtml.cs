using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Components.InputHeader;

public class InputHeader : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        string? header = null,
        string? title = null,
        string? hint = null)
    {
        // We may need to display field title as a page heading when there is only one input on the page.
        // Is important to display vertical red line next to header in case of error.
        // Please use header param when you need to display field title as a page header otherwise use title param.
        return View("InputHeader", (fieldName, header, title, hint));
    }
}
