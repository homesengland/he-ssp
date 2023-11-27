using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.BackLinkWithCallback;

public class BackLinkWithCallback : ViewComponent
{
    public IViewComponentResult Invoke(string url)
    {
        var model = string.IsNullOrWhiteSpace(Request.Query["callbackUrl"]) ? url : Request.Query["callbackUrl"].ToString();
        return View("BackLinkWithCallback", model);
    }
}
