using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.OrganisationDetails;

public class OrganisationDetails : ViewComponent
{
    public IViewComponentResult Invoke(string? street = null, string? city = null, string? postalCode = null, string? providerCode = null)
    {
        return View("OrganisationDetails", (street, city, postalCode, providerCode));
    }
}
