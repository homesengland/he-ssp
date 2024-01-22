using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.SiteConfirmSelectHeader;

public class SiteConfirmSelectHeader : ViewComponent
{
    public IViewComponentResult Invoke(string header, string name, string description)
    {
        return View("SiteConfirmSelectHeader", (header, name, description));
    }
}
