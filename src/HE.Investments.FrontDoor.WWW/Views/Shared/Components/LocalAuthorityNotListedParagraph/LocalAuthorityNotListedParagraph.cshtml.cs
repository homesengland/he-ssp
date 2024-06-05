using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.FrontDoor.WWW.Views.Shared.Components.LocalAuthorityNotListedParagraph;

public class LocalAuthorityNotListedParagraph : ViewComponent
{
    public IViewComponentResult Invoke(string searchLocalAuthorityUrl)
    {
        return View("LocalAuthorityNotListedParagraph", searchLocalAuthorityUrl);
    }
}
