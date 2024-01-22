using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Pagination;

namespace HE.Investment.AHP.Domain.Site.Repositories;

public interface ISiteRepository : ISiteNameExist
{
    Task<PaginationResult<SiteEntity>> GetSites(UserAccount userAccount, PaginationRequest paginationRequest, CancellationToken cancellationToken);

    Task<SiteEntity> GetSite(SiteId siteId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<SiteId> Save(SiteEntity site, UserAccount userAccount, CancellationToken cancellationToken);
}
