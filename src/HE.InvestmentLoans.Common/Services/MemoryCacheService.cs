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
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(_config.ExpireMinutes));

            _memoryCache.Set(key, value, cacheEntryOptions);
        }

        return value;
    }
}
