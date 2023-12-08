using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.Link;

public class Link : ViewComponent
{
    public IViewComponentResult Invoke(string text, string action, string controller, object? values)
    {
        return View("Link", (text, action, controller, values));
    }
}
