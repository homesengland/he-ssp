using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.FormHeader;

public class FormHeader : ViewComponent
{
    public IViewComponentResult Invoke(string title, string? caption = null)
    {
        return View("FormHeader", (Caption: caption, Title: title));
    }
}
