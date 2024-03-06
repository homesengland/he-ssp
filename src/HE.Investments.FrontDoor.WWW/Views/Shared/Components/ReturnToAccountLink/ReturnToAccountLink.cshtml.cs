using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.FrontDoor.WWW.Views.Shared.Components.ReturnToAccountLink;

public class ReturnToAccountLink : ViewComponent
{
    public IViewComponentResult Invoke(bool isEditable = true, bool isLinkInsideForm = true)
    {
        return View("ReturnToAccountLink", (isEditable, isLinkInsideForm));
    }
}
