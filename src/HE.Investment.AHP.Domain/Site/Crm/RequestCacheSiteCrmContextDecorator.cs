using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Infrastructure.Cache;

namespace HE.Investment.AHP.Domain.Site.Crm;

internal class RequestCacheSiteCrmContextDecorator : ISiteCrmContext
{
    private readonly ISiteCrmContext _decorated;

    private readonly InMemoryCache<SiteDto, string> _cache = new();

    public RequestCacheSiteCrmContextDecorator(ISiteCrmContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<PagedResponseDto<SiteDto>> GetOrganisationSites(Guid organisationId, PagingRequestDto pagination, CancellationToken cancellationToken)
    {
        return await _decorated.GetOrganisationSites(organisationId, pagination, cancellationToken);
    }

    public async Task<PagedResponseDto<SiteDto>> GetUserSites(string userGlobalId, PagingRequestDto pagination, CancellationToken cancellationToken)
    {
        return await _decorated.GetUserSites(userGlobalId, pagination, cancellationToken);
    }

    public async Task<SiteDto?> GetById(string siteId, CancellationToken cancellationToken)
    {
        return await _cache.GetFromCache(siteId, async () => await _decorated.GetById(siteId, cancellationToken));
    }

    public async Task<bool> Exist(string name, CancellationToken cancellationToken)
    {
        return await _decorated.Exist(name, cancellationToken);
    }

    public async Task<string> Save(Guid organisationId, string userGlobalId, SiteDto dto, CancellationToken cancellationToken)
    {
        dto.id = await _decorated.Save(organisationId, userGlobalId, dto, cancellationToken);
        _cache.ReplaceCache(dto.id, dto);

        return dto.id;
    }
}
