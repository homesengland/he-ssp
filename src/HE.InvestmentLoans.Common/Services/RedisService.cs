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

    public T? GetValue<T>(string key)
    {
        if (Cache.KeyExists(key))
        {
            string? resp = Cache.StringGet(key);
            return resp != null ? JsonSerializer.Deserialize<T>(resp) : default;
        }

        return default;
    }

    public T? GetValue<T>(string key, Func<T> loadValue)
    {
        if (Cache.KeyExists(key))
        {
            string? resp = Cache.StringGet(key);
            return resp != null ? JsonSerializer.Deserialize<T>(resp) : default;
        }

        var value = loadValue();

        if (value != null)
        {
            SetValue(key, value);
        }

        return value;
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
            SetValue(key, value);
        }

        return value;
    }

    public void SetValue(string key, object value) => SetValue(key, value, _config.ExpireMinutes);

    public void SetValue(string key, object value, int expireMinutes)
    {
        Cache.StringSet(key, JsonSerializer.Serialize(value), TimeSpan.FromMinutes(expireMinutes));
    }

    public void SetValue<T>(string key, T value)
    {
        SetValue(key, (object)value!);
    }
}
