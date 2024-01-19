using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Components.RadioList;

public class RadioList : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        IEnumerable<ExtendedSelectListItem> availableOptions,
        string? header = null,
        string? title = null,
        string? hint = null,
        Enum? value = null)
    {
        return View("RadioList", (fieldName, header, title, hint, availableOptions, value?.ToString()));
    }
}
