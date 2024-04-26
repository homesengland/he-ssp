using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Gds;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.SiteStatusTag;

public class SiteStatusTag : ViewComponent
{
    public IViewComponentResult Invoke(SiteStatus siteStatus)
    {
        return View("SiteStatusTag", (siteStatus, GetColorBaseOnStatus(siteStatus)));
    }

    private static TagColour GetColorBaseOnStatus(SiteStatus siteStatus)
    {
        return siteStatus switch
        {
            SiteStatus.Completed => TagColour.Blue,
            SiteStatus.InProgress => TagColour.Grey,
            _ => TagColour.Grey,
        };
    }
}
