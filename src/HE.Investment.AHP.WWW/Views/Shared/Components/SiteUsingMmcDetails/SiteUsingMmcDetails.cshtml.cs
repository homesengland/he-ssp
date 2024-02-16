using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.SiteUsingMmcDetails;

public class SiteUsingMmcDetails : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("SiteUsingMmcDetails");
    }
}
