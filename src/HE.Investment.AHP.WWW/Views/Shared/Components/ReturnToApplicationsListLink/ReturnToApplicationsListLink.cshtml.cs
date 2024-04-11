using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ReturnToApplicationsListLink;

public class ReturnToApplicationsListLink : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("ReturnToApplicationsListLink");
    }
}
