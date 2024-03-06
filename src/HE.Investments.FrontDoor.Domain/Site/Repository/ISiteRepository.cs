using HE.Investments.Account.Shared.User;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Site;

namespace HE.Investments.FrontDoor.Domain.Site.Repository;

public interface ISiteRepository
{
    Task<IList<ProjectSiteEntity>> GetProjects(FrontDoorProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<ProjectSiteEntity> GetSite(FrontDoorSiteId siteId, FrontDoorProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<ProjectSiteEntity> Save(ProjectSiteEntity project, UserAccount userAccount, CancellationToken cancellationToken);
}
