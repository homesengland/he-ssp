using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace HE.InvestmentLoans.Common.Services;

public class MemoryCacheService : ICacheService
{
    private readonly ICacheConfig _config;

    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService(ICacheConfig config, IMemoryCache memoryCache)
    {
        _config = config;
        _memoryCache = memoryCache;
    }

    public T? GetValue<T>(string key, Func<T> loadValue)
    {
        if (_memoryCache.TryGetValue(key, out T cacheValue))
        {
            return cacheValue;
        }

        var value = loadValue();

        if (value != null)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(_config.ExpireMinutes));

            _memoryCache.Set(key, value, cacheEntryOptions);
        }

        return value;
    }

    public T? GetValue<T>(string key)
    {
        if (_memoryCache.TryGetValue(key, out T cacheValue))
        {
            return cacheValue;
        }

        return default;
    }

    public void SetValue<T>(string key, T value)
    {
        if (value != null)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(_config.ExpireMinutes));
            _memoryCache.Set(key, value, cacheEntryOptions);
        }
    }

    public async Task<T?> GetValueAsync<T>(string key, Func<Task<T>> loadValue)
    {
        if (_memoryCache.TryGetValue(key, out T cacheValue))
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
}
