using HE.Investments.Common.WWW.Components;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.OrganisationDetailsComponent;

public class OrganisationDetailsComponent : ViewComponent
{
    public static DynamicComponentViewModel FromOrganisationDetails(HE.Investments.AHP.Consortium.Contract.OrganisationDetails organisation)
    {
        return new DynamicComponentViewModel(
            nameof(OrganisationDetailsComponent),
            new
            {
                name = organisation.Name,
                street = organisation.Street,
                city = organisation.City,
                postalCode = organisation.PostalCode,
                companyHouseNumber = organisation.CompanyHouseNumber,
            });
    }

    public IViewComponentResult Invoke(string? name = null, string? street = null, string? city = null, string? postalCode = null, string? providerCode = null, string? companyHouseNumber = null)
    {
        return View("OrganisationDetailsComponent", (name, street, city, postalCode, providerCode, companyHouseNumber));
    }
}
