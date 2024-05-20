using HE.Investment.AHP.WWW.Models.Common;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.WWW.Components.SelectList;

namespace HE.Investment.AHP.WWW.Extensions;

public static class PagedConsortiumMemberDetailsExtensions
{
    public static PaginationResult<SelectListItemViewModel> ToSelectListViewModel(this PaginationResult<ConsortiumMemberDetails> consortiumMembers, Func<OrganisationId, string?> urlFactory)
    {
        return new PaginationResult<SelectListItemViewModel>(
            consortiumMembers.Items.Select(x => new PartnerSelectListItemViewModel(
                    urlFactory(x.OrganisationId) ?? string.Empty,
                    x))
                .ToList<SelectListItemViewModel>(),
            consortiumMembers.CurrentPage,
            consortiumMembers.ItemsPerPage,
            consortiumMembers.TotalItems);
    }
}
