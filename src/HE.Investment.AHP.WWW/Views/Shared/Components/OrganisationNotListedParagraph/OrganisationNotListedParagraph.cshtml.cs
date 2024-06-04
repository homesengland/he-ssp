using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.OrganisationNotListedParagraph;

public class OrganisationNotListedParagraph : ViewComponent
{
    public IViewComponentResult Invoke(string searchOrganisationUrl, string enterDetailsUrl)
    {
        return View("OrganisationNotListedParagraph", (searchOrganisationUrl, enterDetailsUrl));
    }
}
