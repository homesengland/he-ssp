using HE.Investments.Common.Infrastructure.Cache.Config;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace HE.Investments.Common.Infrastructure.Cache;

public class MemoryCacheService : ICacheService
{
    private readonly ICacheConfig _config;

    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService(ICacheConfig config, IMemoryCache memoryCache)
    {
        _config = config;
        _memoryCache = memoryCache;
    }

    public T? GetValue<T>(string key) => _memoryCache.TryGetValue(key, out T? cacheValue) ? cacheValue : default;

    public async Task<T?> GetValueAsync<T>(string key, Func<Task<T>> loadValue)
    {
        if (_memoryCache.TryGetValue(key, out T? cacheValue))
        {
            return cacheValue;
        }

        var value = await loadValue();

        if (value != null)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(_config.ExpireMinutes));

            _memoryCache.Set(key, value, cacheEntryOptions);
        }

        return value;
    }

    public void SetValue<T>(string key, T value)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(_config.ExpireMinutes));
        _memoryCache.Set(key, value, cacheEntryOptions);
    }

    public Task SetValueAsync<T>(string key, T value)
    {
        SetValue(key, value);
        return Task.CompletedTask;
    }

    public void Delete(string key)
    {
        _memoryCache.Remove(key);
    }

    public Task DeleteAsync(string key)
    {
        _memoryCache.Remove(key);
        return Task.CompletedTask;
    }
}
