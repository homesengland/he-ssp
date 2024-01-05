using HE.Investments.Common.WWW.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HE.Investments.Common.WWW.Components.Details;

public class Details : ViewComponent
{
    public IViewComponentResult Invoke(string title, string? contentText = null, ComponentViewModel? contentComponent = null)
    {
        return View("Details", (title, contentText, contentComponent));
    }
}
