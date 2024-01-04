using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.RadioYesNo;

public class RadioYesNo : ViewComponent
{
    public IViewComponentResult Invoke(string fieldName, string? header = null, string? title = null, string? hint = null, bool? value = null)
    {
        return View("RadioYesNo", (fieldName, header, title, hint, value));
    }
}
