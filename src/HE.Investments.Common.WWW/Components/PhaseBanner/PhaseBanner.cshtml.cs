using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.PhaseBanner;

public class PhaseBanner : ViewComponent
{
    public IViewComponentResult Invoke(bool displayBanner = true, string? phaseName = null)
    {
        return View("PhaseBanner", (displayBanner, phaseName));
    }
}
