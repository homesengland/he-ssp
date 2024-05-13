using HE.Investment.AHP.WWW.Views.Shared.Components.OrganisationDetailsComponent;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Components.SelectList;

namespace HE.Investment.AHP.WWW.Models.Site;

public record PartnerSelectListItemViewModel(string Url, ConsortiumMemberDetails Partner)
    : SelectListItemViewModel(
        Url,
        Partner.Details.Name,
        null,
        new DynamicComponentViewModel(
            nameof(OrganisationDetailsComponent),
            new
            {
                street = Partner.Details.Street,
                city = Partner.Details.City,
                postalCode = Partner.Details.PostalCode,
                companyHouseNumber = Partner.Details.CompanyHouseNumber,
            }));
