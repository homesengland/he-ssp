using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Components.InputHeader;

public class InputHeader : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        string? label = null,
        string? hint = null)
    {
        return View("InputHeader", (fieldName, label, hint));
    }
}
