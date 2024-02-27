extern alias Org;

using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Data;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Mappers;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.Repositories;

public class SiteRepository : ISiteRepository
{
    private readonly ISiteCrmContext _siteCrmContext;

    public SiteRepository(ISiteCrmContext siteCrmContext)
    {
        _siteCrmContext = siteCrmContext;
    }

    public Task<bool> IsExist(SiteName name, SiteId exceptSiteId, CancellationToken cancellationToken)
    {
        // TODO: #87822 - check availability
        return Task.FromResult(false);
    }

    public async Task<PaginationResult<SiteEntity>> GetSites(UserAccount userAccount, PaginationRequest paginationRequest, CancellationToken cancellationToken)
    {
        var sites = await _siteCrmContext.GetAll(cancellationToken);

        return new PaginationResult<SiteEntity>(
            sites.TakePage(paginationRequest).Select(SiteDtoToSiteEntityMapper.Map).ToList(),
            paginationRequest.Page,
            paginationRequest.ItemsPerPage,
            sites.Count);
    }

    public async Task<SiteEntity> GetSite(SiteId siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (siteId.IsNew)
        {
            return SiteEntity.NewSite();
        }

        var site = await _siteCrmContext.GetById(siteId.Value, cancellationToken);

        if (site == null)
        {
            throw new NotFoundException("Site not found", siteId);
        }

        return SiteDtoToSiteEntityMapper.Map(site);
    }

    public async Task<SiteId> Save(SiteEntity site, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (!site.IsModified)
        {
            return site.Id;
        }

        var id = await _siteCrmContext.Save(SiteEntityToSiteDtoMapper.Map(site), cancellationToken);
        return new SiteId(id);
    }
}
