using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Infrastructure.Cache;

namespace HE.Investments.FrontDoor.Domain.Site.Crm;

internal class RequestCacheSiteCrmContextDecorator : ISiteCrmContext
{
    private readonly InMemoryCache<FrontDoorProjectSiteDto, string> _siteCache = new();

    private readonly InMemoryCache<PagedResponseDto<FrontDoorProjectSiteDto>, string> _sitesCache = new();

    private readonly ISiteCrmContext _decorated;

    public RequestCacheSiteCrmContextDecorator(ISiteCrmContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<PagedResponseDto<FrontDoorProjectSiteDto>> GetSites(string projectId, UserAccount userAccount, PagingRequestDto pagination, CancellationToken cancellationToken)
    {
        return (await _sitesCache.GetFromCache(
            $"{ShortGuid.ToGuidAsString(projectId)}-{pagination.pageNumber}-{pagination.pageSize}",
            async () => await _decorated.GetSites(projectId, userAccount, pagination, cancellationToken)))!;
    }

    public async Task<FrontDoorProjectSiteDto> GetSite(string projectId, string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        return (await _siteCache.GetFromCache(
            ShortGuid.ToGuidAsString(siteId),
            async () => await _decorated.GetSite(projectId, siteId, userAccount, cancellationToken)))!;
    }

    public async Task<string> Save(string projectId, FrontDoorProjectSiteDto dto, UserAccount userAccount, CancellationToken cancellationToken)
    {
        dto.SiteId = await _decorated.Save(projectId, dto, userAccount, cancellationToken);
        _siteCache.ReplaceCache(dto.SiteId, dto);
        _sitesCache.Purge();

        return dto.SiteId;
    }

    public async Task<string> Remove(string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        _siteCache.Delete(siteId);
        _sitesCache.Purge();

        return await _decorated.Remove(siteId, userAccount, cancellationToken);
    }
}
