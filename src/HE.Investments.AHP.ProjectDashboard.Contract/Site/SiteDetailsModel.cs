using HE.Investment.AHP.Contract.Site;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.AHP.ProjectDashboard.Contract.Site;

public record SiteDetailsModel(
    SiteId SiteId,
    FrontDoorProjectId ProjectId,
    string SiteName,
    string OrganisationName,
    string? LocalAuthorityName,
    IList<ApplicationSiteModel> Applications,
    IList<AllocationSiteModel> Allocations);
