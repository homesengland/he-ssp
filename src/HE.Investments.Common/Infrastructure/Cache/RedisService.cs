using System.Text.Json;
using HE.Investments.Common.Infrastructure.Cache.Config;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using StackExchange.Redis;

namespace HE.Investments.Common.Infrastructure.Cache;

public class RedisService : ICacheService
{
    private readonly ICacheConfig _cacheConfig;

    private readonly ConnectionMultiplexer _connection;

    public RedisService(ICacheConfig cacheConfig, ConfigurationOptions options)
    {
        _cacheConfig = cacheConfig;
        _connection = ConnectionMultiplexer.Connect(options);
    }

    private IDatabase Cache => _connection.GetDatabase();

    public T? GetValue<T>(string key)
    {
        if (Cache.KeyExists(key))
        {
            string? resp = Cache.StringGet(key);
            return resp != null ? Deserialize<T>(resp) : default;
        }

        return default;
    }

    public async Task<T?> GetValueAsync<T>(string key, Func<Task<T>> loadValue)
    {
        if (Cache.KeyExists(key))
        {
            return GetValue<T>(key);
        }

        var value = await loadValue();

        if (value != null)
        {
            await SetValueAsync(key, value);
        }

        return value;
    }

    public void SetValue(string key, object value) => SetValue(key, value, _cacheConfig.ExpireMinutes);

    public void SetValue(string key, object value, int expireMinutes)
    {
        Cache.StringSet(key, Serialize(value), TimeSpan.FromMinutes(expireMinutes));
    }

    public void SetValue<T>(string key, T value)
    {
        SetValue(key, (object)value!);
    }

    public async Task SetValueAsync<T>(string key, T value)
    {
        await Cache.StringSetAsync(key, Serialize(value), TimeSpan.FromMinutes(_cacheConfig.ExpireMinutes));
    }

    public void Delete(string key)
    {
        Cache.KeyDelete(key);
    }

    public async Task DeleteAsync(string key)
    {
        await Cache.KeyDeleteAsync(key);
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
