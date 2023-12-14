using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Components.RadioListHeader;

public class RadioListHeader : ViewComponent
{
    public IViewComponentResult Invoke(
        string? title = null,
        string? hint = null)
    {
        return View("RadioListHeader", (title, hint));
    }
}
