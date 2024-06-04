using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.SectionSummary;

public class SectionSummary : ViewComponent
{
    public IViewComponentResult Invoke(string title, IList<SectionSummaryItemModel>? items, string name = "", bool hasError = false)
    {
        return View("SectionSummary", (title, items ?? [], name, hasError));
    }
}
