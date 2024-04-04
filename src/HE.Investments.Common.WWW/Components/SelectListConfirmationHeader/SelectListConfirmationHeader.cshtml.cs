using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.SelectListConfirmationHeader;

public class SelectListConfirmationHeader : ViewComponent
{
    public IViewComponentResult Invoke(string header, string name, string description)
    {
        return View("SelectListConfirmationHeader", (header, name, description));
    }
}
