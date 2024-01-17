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
        IEnumerable<ExtendedSelectListItem> availableOptions,
        ExtendedSelectListItem alternativeOption,
        Enum? value = null)
    {
        return View("RadioListWithOr", (fieldName, title, hint, availableOptions, alternativeOption, value?.ToString()));
    }
}
