using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investments.Account.Shared.User;

namespace HE.Investment.AHP.Domain.Site.Repositories;

public interface ISiteRepository
{
    Task<IList<SiteEntity>> GetSites(UserAccount userAccount, CancellationToken cancellationToken);

    Task<SiteEntity> GetSite(UserAccount userAccount, string siteId);
}
