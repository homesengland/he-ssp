using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Components.SelectList;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Organisation.Contract;

namespace HE.Investment.AHP.WWW.Models.Common;

public record SearchOrganisationResultModel(
    string Phrase,
    string NavigationUrl,
    PaginationResult<OrganisationDetails> Result,
    string SelectedMember)
{
    public PaginationResult<ExtendedSelectListItem> ToExtendedSelectListItems() => new(
        Result.Items.Select(x => new ExtendedSelectListItem(
                x.Name,
                x.OrganisationId ?? x.CompanyHouseNumber ?? string.Empty,
                false,
                itemContent: OrganisationDetailsComponent(x)))
            .ToList(),
        Result.CurrentPage,
        Result.ItemsPerPage,
        Result.TotalItems);

    public PaginationResult<SelectListItemViewModel> ToSelectListItemViewModel(Func<string, string?> urlFactory) => new(
        Result.Items.Select(x => new SelectListItemViewModel(
                urlFactory(x.OrganisationId ?? x.CompanyHouseNumber ?? string.Empty) ?? string.Empty,
                x.Name,
                null,
                OrganisationDetailsComponent(x)))
            .ToList(),
        Result.CurrentPage,
        Result.ItemsPerPage,
        Result.TotalItems);

    private static DynamicComponentViewModel OrganisationDetailsComponent(OrganisationDetails details) => new(
        nameof(Views.Shared.Components.OrganisationDetailsComponent.OrganisationDetailsComponent),
        new
        {
            street = details.Street,
            city = details.City,
            postalCode = details.PostalCode,
            providerCode = (string?)null,
            companyHouseNumber = details.CompanyHouseNumber,
        });
}
