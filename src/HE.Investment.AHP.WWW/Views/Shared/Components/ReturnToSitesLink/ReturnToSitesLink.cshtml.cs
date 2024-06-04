using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ReturnToSitesLink;

public class ReturnToSitesLink : ViewComponent
{
    public IViewComponentResult Invoke(string projectId, bool isEditable = true, bool isLinkInsideForm = true)
    {
        return View("ReturnToSitesLink", (projectId, isEditable, isLinkInsideForm));
    }
}
