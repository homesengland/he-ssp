using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Components.RadioListWithOr;

public class RadioListWithOr : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        IList<ExtendedSelectListItem> availableOptions,
        ExtendedSelectListItem alternativeOption)
    {
        return View("RadioListWithOr", (fieldName, availableOptions, alternativeOption));
    }
}
