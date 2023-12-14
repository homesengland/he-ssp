using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Components.RadioListWithOr;

public class RadioListWithOr : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        string title,
        string hint,
        IList<ExtendedSelectListItem> availableOptions,
        ExtendedSelectListItem alternativeOption)
    {
        return View("RadioListWithOr", (fieldName, title, hint, availableOptions, alternativeOption));
    }
}
