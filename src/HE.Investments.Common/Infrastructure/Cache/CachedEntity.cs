using HE.Investments.Common.Infrastructure.Cache.Interfaces;

namespace HE.Investments.Common.Infrastructure.Cache;

public sealed class CachedEntity<TEntity>
    where TEntity : class
{
    private readonly ICacheService _cacheService;

    private readonly string _cacheKey;

    private readonly Func<Task<TEntity>> _loadEntity;

    private TEntity? _entity;

    public CachedEntity(ICacheService cacheService, string cacheKey, Func<Task<TEntity>> loadEntity)
    {
        _cacheKey = cacheKey;
        _loadEntity = loadEntity;
        _cacheService = cacheService;
        _entity = null;
    }

    public async ValueTask<TEntity?> GetAsync()
    {
        return _entity ??= await _cacheService.GetValueAsync(_cacheKey, _loadEntity);
    }

    public async Task InvalidateAsync()
    {
        _entity = await _loadEntity();
        await _cacheService.SetValueAsync(_cacheKey, _entity);
    }
}
