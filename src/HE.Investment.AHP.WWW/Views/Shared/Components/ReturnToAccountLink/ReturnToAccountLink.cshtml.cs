using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ReturnToAccountLink;

public class ReturnToAccountLink : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("ReturnToAccountLink");
    }
}
