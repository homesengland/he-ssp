using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Infrastructure.Cache;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm;

internal class RequestCacheHomeTypeCrmContextDecorator : IHomeTypeCrmContext
{
    private readonly InMemoryCache<HomeTypeDto, string> _cache = new();

    private readonly IHomeTypeCrmContext _decorated;

    public RequestCacheHomeTypeCrmContextDecorator(IHomeTypeCrmContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<IList<HomeTypeDto>> GetAllOrganisationHomeTypes(string applicationId, Guid organisationId, CancellationToken cancellationToken)
    {
        return await _decorated.GetAllOrganisationHomeTypes(applicationId, organisationId, cancellationToken);
    }

    public async Task<IList<HomeTypeDto>> GetAllUserHomeTypes(string applicationId, Guid organisationId, CancellationToken cancellationToken)
    {
        return await _decorated.GetAllUserHomeTypes(applicationId, organisationId, cancellationToken);
    }

    public async Task<HomeTypeDto?> GetOrganisationHomeTypeById(string applicationId, string homeTypeId, Guid organisationId, CancellationToken cancellationToken)
    {
        return await _cache.GetFromCache(
            homeTypeId,
            async () => await _decorated.GetOrganisationHomeTypeById(applicationId, homeTypeId, organisationId, cancellationToken));
    }

    public async Task<HomeTypeDto?> GetUserHomeTypeById(string applicationId, string homeTypeId, Guid organisationId, CancellationToken cancellationToken)
    {
        return await _cache.GetFromCache(
            homeTypeId,
            async () => await _decorated.GetUserHomeTypeById(applicationId, homeTypeId, organisationId, cancellationToken));
    }

    public async Task Remove(string applicationId, string homeTypeId, Guid organisationId, CancellationToken cancellationToken)
    {
        await _decorated.Remove(applicationId, homeTypeId, organisationId, cancellationToken);
        _cache.Delete(homeTypeId);
    }

    public async Task<string> Save(HomeTypeDto homeType, Guid organisationId, CancellationToken cancellationToken)
    {
        homeType.id = await _decorated.Save(homeType, organisationId, cancellationToken);
        _cache.ReplaceCache(homeType.id, homeType);

        return homeType.id;
    }
}
