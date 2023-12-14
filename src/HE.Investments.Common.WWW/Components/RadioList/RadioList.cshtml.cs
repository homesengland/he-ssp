using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Components.RadioList;

public class RadioList : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        IList<ExtendedSelectListItem> availableOptions,
        string? title = null,
        string? hint = null)
    {
        return View("RadioList", (fieldName, title, hint, availableOptions));
    }
}
