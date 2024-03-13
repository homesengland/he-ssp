using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Views.Shared.Components.SelectedOrganisationInformation;

public class SelectedOrganisationInformation : ViewComponent
{
    public IViewComponentResult Invoke(string name, string companiesHouseNumber, string? street = null, string? city = null, string? postalCode = null)
    {
        return View("SelectedOrganisationInformation", (name, street, city, postalCode, companiesHouseNumber));
    }
}
