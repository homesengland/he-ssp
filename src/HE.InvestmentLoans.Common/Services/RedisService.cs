using System.Text.Json;
using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Services.Interfaces;
using StackExchange.Redis;

namespace HE.InvestmentLoans.Common.Services;

public class RedisService : ICacheService
{
    private readonly ICacheConfig _config;

    private readonly ConnectionMultiplexer _connection;

    public RedisService(ICacheConfig config)
    {
        _config = config;
        _connection = ConnectionMultiplexer.Connect(config.RedisConnectionString);
    }

    private IDatabase Cache => _connection.GetDatabase();

    public T? GetValue<T>(string key, Func<T> loadValue)
    {
        if (Cache.KeyExists(key))
        {
            return GetValue<T>(key);
        }

        var value = loadValue();

        if (value != null)
        {
            SetValue(key, value);
        }

        return value;
    }

    public T? GetValue<T>(string key)
    public async Task<T?> GetValueAsync<T>(string key, Func<Task<T>> loadValue)
    {
        if (Cache.KeyExists(key))
        {
            return ObjectGet<T>(key);
        }

        var value = await loadValue();

        if (value != null)
        {
            ObjectSet(key, value);
        }

        return value;
    }

    private T? ObjectGet<T>(string key)
    {
        string? resp = Cache.StringGet(key);
        return resp != null ? JsonSerializer.Deserialize<T>(resp) : default;
    }

    public void SetValue<T>(string key, T value)
    {
        Cache.StringSet(key, JsonSerializer.Serialize(value), TimeSpan.FromMinutes(_config.ExpireMinutes));
    }
}
