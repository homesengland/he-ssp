using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.RadioYesNo;

public class RadioYesNo : ViewComponent
{
    public IViewComponentResult Invoke(string fieldName, string hint)
    {
        return View("RadioYesNo", (fieldName, hint));
    }
}
