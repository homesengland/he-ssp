using HE.Investments.Common.WWW.Models.Summary;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.FrontDoor.WWW.Views.Shared.Components.ProjectSitesSectionSummary;

public class ProjectSitesSectionSummary : ViewComponent
{
    public IViewComponentResult Invoke(IList<SectionSummaryViewModel> sites, bool isReadOnly)
    {
        return View("ProjectSitesSectionSummary", (sites, isReadOnly));
    }
}
