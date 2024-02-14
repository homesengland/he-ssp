using HE.Investments.Common.Contract.Pagination;

namespace HE.Investment.AHP.Contract.Site;

public record SiteDetailsModel(
    SiteId SiteId,
    string SiteName,
    string OrganisationName,
    string? LocalAuthorityName,
    PaginationResult<ApplicationSiteModel> Applications);
