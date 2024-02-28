using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.ButtonStart;

public class ButtonStart : ViewComponent
{
    public IViewComponentResult Invoke(string href, string? text = null)
    {
        return View("ButtonStart", (href, text));
    }
}
