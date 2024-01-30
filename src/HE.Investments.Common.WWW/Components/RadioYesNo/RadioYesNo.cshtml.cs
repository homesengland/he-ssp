using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.RadioYesNo;

public class RadioYesNo : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        DynamicComponentViewModel? headerComponent = null,
        string? header = null,
        string? title = null,
        string? hint = null,
        bool? value = null,
        bool? isDisplayed = null)
    {
        return View("RadioYesNo", (fieldName, headerComponent, header, title, hint, value, isDisplayed ?? true));
    }
}
