using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.SummaryList;

public class SummaryList : ViewComponent
{
    public IViewComponentResult Invoke(string header, IEnumerable<SummaryListItem> items, string? paragraph = null)
    {
        return View("SummaryList", (header, paragraph ?? string.Empty, items.ToList()));
    }
}
