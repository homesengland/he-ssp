using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.LocalAuthorityNotListedParagraph;

public class LocalAuthorityNotListedParagraph : ViewComponent
{
    public IViewComponentResult Invoke(string searchOrganisationUrl)
    {
        return View("LocalAuthorityNotListedParagraph", searchOrganisationUrl);
    }
}
