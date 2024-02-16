using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ConsortiumHelpDetailsContent;

public class ConsortiumHelpDetailsContent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("ConsortiumHelpDetailsContent");
    }
}
