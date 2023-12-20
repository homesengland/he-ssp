using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.LinkWorkingAsButton;

public class LinkWorkingAsButton : ViewComponent
{
    public IViewComponentResult Invoke(string text, string name, string value)
    {
        return View("LinkWorkingAsButton", (text, name, value));
    }
}
