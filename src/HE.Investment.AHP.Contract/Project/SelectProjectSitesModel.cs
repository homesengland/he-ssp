using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investment.AHP.Contract.Project;

public record SelectProjectSitesModel(
    ProjectSitesModel ProjectSites,
    string? CallbackUrl);
