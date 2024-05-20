using HE.Investments.Common.Contract.Pagination;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investment.AHP.Contract.Site;

public record SiteDetailsModel(
    SiteId SiteId,
    FrontDoorProjectId ProjectId,
    string SiteName,
    string OrganisationName,
    string? LocalAuthorityName,
    PaginationResult<ApplicationSiteModel> Applications);
