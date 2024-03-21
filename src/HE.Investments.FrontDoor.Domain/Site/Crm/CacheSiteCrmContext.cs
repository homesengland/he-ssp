using System.Collections.Concurrent;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;

namespace HE.Investments.FrontDoor.Domain.Site.Crm;

public class CacheSiteCrmContext : ISiteCrmContext
{
    private readonly ConcurrentDictionary<string, FrontDoorProjectSiteDto> _cache = new();

    private readonly ISiteCrmContext _decorated;

    public CacheSiteCrmContext(ISiteCrmContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<PagedResponseDto<FrontDoorProjectSiteDto>> GetSites(string projectId, UserAccount userAccount, PagingRequestDto pagination, CancellationToken cancellationToken)
    {
        return await _decorated.GetSites(projectId, userAccount, pagination, cancellationToken);
    }

    public Task<FrontDoorProjectSiteDto> GetSite(string projectId, string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        return GetFromCache(projectId, siteId, userAccount, _decorated.GetSite, cancellationToken);
    }

    public async Task<string> Save(string projectId, FrontDoorProjectSiteDto dto, UserAccount userAccount, CancellationToken cancellationToken)
    {
        ReplaceCache(dto);
        return await _decorated.Save(projectId, dto, userAccount, cancellationToken);
    }

    public Task<string> Remove(string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        return _decorated.Remove(siteId, userAccount, cancellationToken);
    }

    private async Task<FrontDoorProjectSiteDto> GetFromCache(string projectId, string siteId, UserAccount userAccount, Func<string, string, UserAccount, CancellationToken, Task<FrontDoorProjectSiteDto>> getFromContext, CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue(siteId, out var site))
        {
            return site;
        }

        var frontDoorSite = await getFromContext(projectId, siteId, userAccount, cancellationToken);
        _cache.TryAdd(siteId, frontDoorSite);

        return frontDoorSite;
    }

    private void ReplaceCache(FrontDoorProjectSiteDto site)
    {
        if (string.IsNullOrEmpty(site.SiteId))
        {
            return;
        }

        _cache.AddOrUpdate(site.SiteId, site, (_, _) => site);
    }
}
