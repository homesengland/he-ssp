using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Components.RadioListContent;

public class RadioListContent : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        IList<ExtendedSelectListItem> availableOptions)
    {
        return View("RadioListContent", (fieldName, availableOptions));
    }
}
