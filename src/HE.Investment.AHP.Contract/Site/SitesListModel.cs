using HE.Investments.Common.Contract.Pagination;

namespace HE.Investment.AHP.Contract.Site;

public record SitesListModel(string OrganisationName, PaginationResult<SiteBasicModel> Page);
