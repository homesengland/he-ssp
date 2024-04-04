using HE.Investments.Account.Shared.User;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Domain.Site.Repository;

public interface IRemoveSiteRepository
{
    Task<string> Remove(FrontDoorSiteId siteId, UserAccount userAccount, CancellationToken cancellationToken);
}
