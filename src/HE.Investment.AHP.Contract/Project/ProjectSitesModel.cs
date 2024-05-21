using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Contract.Pagination;

namespace HE.Investment.AHP.Contract.Project;

public record ProjectSitesModel(
    AhpProjectId ProjectId,
    string ProjectName,
    string OrganisationName,
    PaginationResult<SiteBasicModel> Sites);
