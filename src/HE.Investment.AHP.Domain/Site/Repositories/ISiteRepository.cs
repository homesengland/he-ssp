using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Pagination;

namespace HE.Investment.AHP.Domain.Site.Repositories;

public interface ISiteRepository : ISiteNameExist
{
    Task<PaginationResult<SiteEntity>> GetSites(UserAccount userAccount, PaginationRequest paginationRequest, CancellationToken cancellationToken);

    Task<SiteBasicInfo> GetSiteBasicInfo(SiteId siteId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<SiteEntity> GetSite(SiteId siteId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<bool> IsExist(StrategicSiteName name, UserAccount userAccount, CancellationToken cancellationToken);

    Task<SiteId> Save(SiteEntity site, UserAccount userAccount, CancellationToken cancellationToken);
}
