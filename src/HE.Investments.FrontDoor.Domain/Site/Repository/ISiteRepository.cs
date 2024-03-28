using HE.Investments.Account.Shared.User;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Domain.Site.Repository;

public interface ISiteRepository
{
    Task<ProjectSitesEntity> GetProjectSites(FrontDoorProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<ProjectSiteEntity> GetSite(FrontDoorProjectId projectId, FrontDoorSiteId siteId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<ProjectSiteEntity> Save(ProjectSiteEntity site, UserAccount userAccount, CancellationToken cancellationToken);
}
