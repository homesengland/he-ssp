using System.Text.Json;
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

    public T? GetValue<T>(string key) => _memoryCache.TryGetValue(key, out string? cacheValue) && !string.IsNullOrEmpty(cacheValue)
        ? Deserialize<T>(cacheValue)
        : default;

    public async Task<T?> GetValueAsync<T>(string key, Func<Task<T>> loadValue)
    {
        if (_memoryCache.TryGetValue(key, out string? cacheValue) && !string.IsNullOrEmpty(cacheValue))
        {
            return Deserialize<T>(cacheValue);
        }

        var value = await loadValue();
        if (value != null)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(_config.ExpireMinutes));

            _memoryCache.Set(key, Serialize(value), cacheEntryOptions);
        }

        return value;
    }

    public void SetValue<T>(string key, T value)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(_config.ExpireMinutes));
        _memoryCache.Set(key, Serialize(value), cacheEntryOptions);
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

    private static string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value);
    }

    private static T? Deserialize<T>(string value)
    {
        return JsonSerializer.Deserialize<T>(value);
    }
}
