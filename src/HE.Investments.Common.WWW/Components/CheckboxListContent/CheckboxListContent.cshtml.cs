using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.CheckboxListContent;

public class CheckboxListContent : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        IEnumerable<ExtendedSelectListItem> availableOptions)
    {
        return View("CheckboxListContent", (fieldName, availableOptions));
    }
}
