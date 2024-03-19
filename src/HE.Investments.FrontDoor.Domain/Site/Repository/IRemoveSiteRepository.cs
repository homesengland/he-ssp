using HE.Investments.Account.Shared.User;

namespace HE.Investments.FrontDoor.Domain.Site.Repository;

public interface IRemoveSiteRepository
{
    Task<string> Remove(ProjectSiteEntity site, UserAccount userAccount, CancellationToken cancellationToken);
}
