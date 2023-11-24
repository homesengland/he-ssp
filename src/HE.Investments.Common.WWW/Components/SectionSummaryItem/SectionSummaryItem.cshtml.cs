using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.SectionSummaryItem;

public class SectionSummaryItem : ViewComponent
{
    public IViewComponentResult Invoke(string name, IList<string>? values, string actionUrl, Dictionary<string, string>? files = null, bool isEditable = true, bool isVisible = true)
    {
        return View("SectionSummaryItem", (name, values ?? new List<string>(), actionUrl, files ?? new Dictionary<string, string>(), isEditable, isVisible));
    }
}
