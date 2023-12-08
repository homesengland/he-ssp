using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.SummaryItem;

public class SummaryItem : ViewComponent
{
    public IViewComponentResult Invoke(string title, string? value)
    {
        return View("SummaryItem", (title, value));
    }
}
