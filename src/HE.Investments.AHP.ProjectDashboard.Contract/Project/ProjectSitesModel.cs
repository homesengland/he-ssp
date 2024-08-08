using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.AHP.ProjectDashboard.Contract.Project;

public record ProjectSitesModel(
    FrontDoorProjectId ProjectId,
    string ProjectName,
    string OrganisationName,
    PaginationResult<SiteBasicModel> Sites);
