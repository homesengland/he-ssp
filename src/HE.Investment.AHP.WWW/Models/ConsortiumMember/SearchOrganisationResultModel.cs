using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.WWW.Models;

namespace HE.Investment.AHP.WWW.Models.ConsortiumMember;

public record SearchOrganisationResultModel(
    string ConsortiumId,
    string Phrase,
    string NavigationUrl,
    PaginationResult<ExtendedSelectListItem> Page,
    string? SelectedMember);
