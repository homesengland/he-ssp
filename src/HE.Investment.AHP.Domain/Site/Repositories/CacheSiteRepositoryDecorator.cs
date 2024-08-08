using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Infrastructure.Cache;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.Repositories;

internal sealed class CacheSiteRepositoryDecorator : ISiteRepository
{
    private readonly ISiteRepository _decorated;

    private readonly ICacheService _cache;

    private readonly InMemoryCache<SiteBasicInfoDto, string> _memoryCache = new();

    public CacheSiteRepositoryDecorator(ISiteRepository decorated, ICacheService cache)
    {
        _decorated = decorated;
        _cache = cache;
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

    public async Task<PaginationResult<ApplicationBasicDetails>> GetSiteApplications(SiteId siteId, ConsortiumUserAccount userAccount, PaginationRequest paginationRequest, CancellationToken cancellationToken)
    {
        return await _decorated.GetSiteApplications(siteId, userAccount, paginationRequest, cancellationToken);
    }

    public async Task<bool> IsExist(SiteName name, CancellationToken cancellationToken)
    {
        return await _decorated.IsExist(name, cancellationToken);
    }

    public async Task<bool> IsExist(StrategicSiteName name, UserAccount userAccount, CancellationToken cancellationToken)
    {
        return await _decorated.IsExist(name, userAccount, cancellationToken);
    }

    public async Task<SiteId> Save(SiteEntity site, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var siteId = await _decorated.Save(site, userAccount, cancellationToken);
        var cacheKey = SiteCacheKey(siteId);

        await _cache.DeleteAsync(cacheKey);
        _memoryCache.Delete(cacheKey);

        return siteId;
    }

    private static string SiteCacheKey(SiteId siteId) => $"ahp-site-{siteId.ToGuidAsString()}";

    private static SiteBasicInfoDto ToSiteBasicInfoDto(SiteBasicInfo entity)
    {
        return new SiteBasicInfoDto
        {
            Id = entity.Id.ToGuidAsString(),
            Name = entity.Name.Value,
            Status = entity.Status,
            ProjectId = entity.FrontDoorProjectId?.Value,
            SiteId = entity.FrontDoorSiteId?.Value,
            AcquisitionStatus = entity.LandAcquisitionStatus.Value,
            LocalAuthorityCode = entity.LocalAuthority?.Code.Value,
            LocalAuthorityName = entity.LocalAuthority?.Name,
            UsingMmc = entity.SiteUsingModernMethodsOfConstruction,
        };
    }

    private static SiteBasicInfo ToSiteBasicInfoEntity(SiteBasicInfoDto dto)
    {
        var localAuthority = !string.IsNullOrEmpty(dto.LocalAuthorityCode) && !string.IsNullOrEmpty(dto.LocalAuthorityName)
            ? new LocalAuthority(new LocalAuthorityCode(dto.LocalAuthorityCode), dto.LocalAuthorityName)
            : null;

        return new SiteBasicInfo(
            SiteId.From(dto.Id),
            new SiteName(dto.Name),
            dto.Status,
            FrontDoorProjectId.Create(dto.ProjectId),
            FrontDoorSiteId.Create(dto.SiteId),
            new LandAcquisitionStatus(dto.AcquisitionStatus),
            localAuthority,
            dto.UsingMmc);
    }

    private sealed class SiteBasicInfoDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public SiteStatus Status { get; set; }

        public string? ProjectId { get; set; }

        public string? SiteId { get; set; }

        public string? LocalAuthorityCode { get; set; }

        public string? LocalAuthorityName { get; set; }

        public SiteLandAcquisitionStatus? AcquisitionStatus { get; set; }

        public SiteUsingModernMethodsOfConstruction UsingMmc { get; set; }
    }
}
