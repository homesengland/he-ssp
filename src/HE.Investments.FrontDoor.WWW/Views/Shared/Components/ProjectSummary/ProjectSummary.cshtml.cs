using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWW.Models.Summary;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.FrontDoor.WWW.Views.Shared.Components.ProjectSummary;

public class ProjectSummary : ViewComponent
{
    public IViewComponentResult Invoke(IList<SectionSummaryViewModel> sections, bool isSiteIdentified)
    {
        return View("ProjectSummary", (sections, isSiteIdentified));
    }
}
