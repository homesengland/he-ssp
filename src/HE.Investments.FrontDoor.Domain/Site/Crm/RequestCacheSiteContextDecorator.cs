using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Cache;

namespace HE.Investments.FrontDoor.Domain.Site.Crm;

internal sealed class RequestCacheSiteContextDecorator : ISiteContext
{
    private readonly InMemoryCache<FrontDoorProjectSiteDto, string> _siteCache = new();

    private readonly InMemoryCache<PagedResponseDto<FrontDoorProjectSiteDto>, string> _sitesCache = new();

    private readonly ISiteContext _decorated;

    public RequestCacheSiteContextDecorator(ISiteContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<PagedResponseDto<FrontDoorProjectSiteDto>> GetSites(string projectId, UserAccount userAccount, PagingRequestDto pagination, CancellationToken cancellationToken)
    {
        return (await _sitesCache.GetFromCache(
            $"{projectId.ToGuidAsString()}-{pagination.pageNumber}-{pagination.pageSize}",
            async () => await _decorated.GetSites(projectId, userAccount, pagination, cancellationToken)))!;
    }

    public async Task<FrontDoorProjectSiteDto> GetSite(string projectId, string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        return (await _siteCache.GetFromCache(
            siteId.ToGuidAsString(),
            async () => await _decorated.GetSite(projectId, siteId, userAccount, cancellationToken)))!;
    }

    public async Task<string> Save(string projectId, FrontDoorProjectSiteDto dto, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        dto.SiteId = await _decorated.Save(projectId, dto, userGlobalId, organisationId, cancellationToken);
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
