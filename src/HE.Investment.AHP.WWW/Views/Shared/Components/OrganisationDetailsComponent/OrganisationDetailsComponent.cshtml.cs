using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.OrganisationDetailsComponent;

public class OrganisationDetailsComponent : ViewComponent
{
    public IViewComponentResult Invoke(string? street = null, string? city = null, string? postalCode = null, string? providerCode = null)
    {
        return View("OrganisationDetailsComponent", (street, city, postalCode, providerCode));
    }
}
