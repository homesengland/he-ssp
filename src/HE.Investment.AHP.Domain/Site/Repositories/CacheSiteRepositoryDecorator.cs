using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Infrastructure.Cache;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investment.AHP.Domain.Site.Repositories;

internal class CacheSiteRepositoryDecorator : ISiteRepository
{
    private readonly ISiteRepository _decorated;

    private readonly ICacheService _cache;

    private readonly InMemoryCache<SiteBasicInfoDto, string> _memoryCache = new();

    public CacheSiteRepositoryDecorator(ISiteRepository decorated, ICacheService cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    public async Task<bool> IsExist(SiteName name, CancellationToken cancellationToken)
    {
        return await _decorated.IsExist(name, cancellationToken);
    }

    public async Task<PaginationResult<SiteEntity>> GetSites(UserAccount userAccount, PaginationRequest paginationRequest, CancellationToken cancellationToken)
    {
        return await _decorated.GetSites(userAccount, paginationRequest, cancellationToken);
    }

    public async Task<SiteBasicInfo> GetSiteBasicInfo(SiteId siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var cacheKey = SiteCacheKey(siteId);
        var siteDto = await _memoryCache.GetFromCache(
            cacheKey,
            async () => await _cache.GetValueAsync(
                cacheKey,
                async () => ToSiteBasicInfoDto(await _decorated.GetSiteBasicInfo(siteId, userAccount, cancellationToken))));

        return ToSiteBasicInfoEntity(siteDto!);
    }

    public async Task<SiteEntity> GetSite(SiteId siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        return await _decorated.GetSite(siteId, userAccount, cancellationToken);
    }

    public async Task<SiteId> Save(SiteEntity site, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var siteId = await _decorated.Save(site, userAccount, cancellationToken);
        var cacheKey = SiteCacheKey(siteId);

        await _cache.DeleteAsync(cacheKey);
        _memoryCache.Delete(cacheKey);

        return siteId;
    }

    private static string SiteCacheKey(SiteId siteId) => $"ahp-site-{siteId.Value}";

    private static SiteBasicInfoDto ToSiteBasicInfoDto(SiteBasicInfo entity)
    {
        return new SiteBasicInfoDto
        {
            Id = entity.Id.Value,
            Name = entity.Name.Value,
            ProjectId = entity.FrontDoorProjectId?.Value,
            SiteId = entity.FrontDoorSiteId?.Value,
            AcquisitionStatus = entity.LandAcquisitionStatus.Value,
            UsingMmc = entity.SiteUsingModernMethodsOfConstruction,
        };
    }

    private static SiteBasicInfo ToSiteBasicInfoEntity(SiteBasicInfoDto dto)
    {
        return new SiteBasicInfo(
            new SiteId(dto.Id),
            new SiteName(dto.Name),
            FrontDoorProjectId.Create(dto.ProjectId),
            FrontDoorSiteId.Create(dto.SiteId),
            new LandAcquisitionStatus(dto.AcquisitionStatus),
            dto.UsingMmc);
    }

    private sealed class SiteBasicInfoDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string? ProjectId { get; set; }

        public string? SiteId { get; set; }

        public SiteLandAcquisitionStatus? AcquisitionStatus { get; set; }

        public SiteUsingModernMethodsOfConstruction UsingMmc { get; set; }
    }
}
