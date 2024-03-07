using HE.Investments.Account.Shared.User;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Site.Repository;

public class SiteRepository : ISiteRepository
{
    public Task<IList<ProjectSiteEntity>> GetSites(FrontDoorProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        IList<ProjectSiteEntity> mocked =
            new List<ProjectSiteEntity>(new[] { new ProjectSiteEntity(new FrontDoorSiteId("456"), projectId, new SiteName("Site test")) });

        return Task.FromResult(mocked);
    }

    public Task<ProjectSiteEntity> GetSite(FrontDoorSiteId siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        return Task.FromResult(new ProjectSiteEntity(siteId, new FrontDoorProjectId("123"), new SiteName("Site test")));
    }

    public Task<ProjectSiteEntity> Save(ProjectSiteEntity project, UserAccount userAccount, CancellationToken cancellationToken)
    {
        return Task.FromResult(project);
    }
}
