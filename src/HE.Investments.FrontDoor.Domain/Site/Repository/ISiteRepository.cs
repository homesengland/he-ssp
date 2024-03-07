using HE.Investments.Account.Shared.User;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Site;

namespace HE.Investments.FrontDoor.Domain.Site.Repository;

public interface ISiteRepository
{
    Task<ProjectSitesEntity> GetSites(FrontDoorProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<ProjectSiteEntity> GetSite(FrontDoorProjectId projectId, FrontDoorSiteId siteId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<ProjectSiteEntity> Save(ProjectSiteEntity site, UserAccount userAccount, CancellationToken cancellationToken);
}
