using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.OrganisationDetailsComponent;

public class OrganisationDetailsComponent : ViewComponent
{
    public IViewComponentResult Invoke(string? name = null, string? street = null, string? city = null, string? postalCode = null, string? providerCode = null, string? companyHouseNumber = null)
    {
        return View("OrganisationDetailsComponent", (name, street, city, postalCode, providerCode, companyHouseNumber));
    }
}
