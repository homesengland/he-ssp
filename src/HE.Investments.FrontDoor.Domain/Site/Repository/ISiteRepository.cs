using HE.Investments.Account.Shared.User;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Site;

namespace HE.Investments.FrontDoor.Domain.Site.Repository;

public interface ISiteRepository
{
    Task<IList<ProjectSiteEntity>> GetSites(FrontDoorProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<ProjectSiteEntity> GetSite(FrontDoorSiteId siteId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<ProjectSiteEntity> Save(ProjectSiteEntity project, UserAccount userAccount, CancellationToken cancellationToken);
}
