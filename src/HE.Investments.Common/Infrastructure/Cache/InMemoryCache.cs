using System.Collections.Concurrent;

namespace HE.Investments.Common.Infrastructure.Cache;

public class InMemoryCache<TEntity, TKey>
    where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, TEntity> _cache = new();

    public async Task<TEntity?> GetFromCache(TKey key, Func<Task<TEntity?>> load)
    {
        if (_cache.TryGetValue(key, out var entity))
        {
            return entity;
        }

        entity = await load();
        if (entity != null)
        {
            _cache.TryAdd(key, entity);
        }

        return entity;
    }

    public void ReplaceCache(TKey key, TEntity entity)
    {
        _cache.AddOrUpdate(key, entity, (_, _) => entity);
    }

    public void Delete(TKey key)
    {
        _cache.TryRemove(key, out _);
    }
}
