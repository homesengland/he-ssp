using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investments.Account.Shared.User;

namespace HE.Investment.AHP.Domain.Site.Repositories;

public interface ISiteRepository : ISiteNameExist
{
    Task<IList<SiteEntity>> GetSites(UserAccount userAccount, CancellationToken cancellationToken);

    Task<SiteEntity> GetSite(SiteId siteId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<SiteId> Save(SiteEntity site, UserAccount userAccount, CancellationToken cancellationToken);
}
