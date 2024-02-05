using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Components.InputListHeader;

public class InputListHeader : ViewComponent
{
    public IViewComponentResult Invoke(
        DynamicComponentViewModel? headerComponent = null,
        string? header = null,
        string? title = null,
        string? hint = null)
    {
        if (headerComponent != null &&
        (!string.IsNullOrWhiteSpace(header) || !string.IsNullOrWhiteSpace(title) || !string.IsNullOrWhiteSpace(hint)))
        {
            throw new ArgumentException("InputListHeader requires headerComponent or header string or combination of title and hint.");
        }

        return View("InputListHeader", (headerComponent, header, title, hint));
    }
}
