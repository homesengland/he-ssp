using System.Text.Json;
using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Services.Interfaces;
using StackExchange.Redis;

namespace HE.InvestmentLoans.Common.Services;

public class RedisService : ICacheService
{
    private readonly string _envSufix;

    private readonly ICacheConfig _config;

    private readonly ConnectionMultiplexer _connection;

    public RedisService(ICacheConfig config, IDataverseConfig dataverseConfig)
    {
        _config = config;
        _connection = ConnectionMultiplexer.Connect(config.RedisConnectionString);
        _envSufix = dataverseConfig.BaseUri!;
    }

    private IDatabase Cache => _connection.GetDatabase();

    public T? GetValue<T>(string key)
    {
        if (Cache.KeyExists(EnvKey(key)))
        {
            string? resp = Cache.StringGet(EnvKey(key));
            return resp != null ? JsonSerializer.Deserialize<T>(resp) : default;
        }

        return default;
    }

    public async Task<T?> GetValueAsync<T>(string key, Func<Task<T>> loadValue)
    {
        if (Cache.KeyExists(EnvKey(key)))
        {
            return GetValue<T>(EnvKey(key));
        }

        var value = await loadValue();

        if (value != null)
        {
            SetValue(EnvKey(key), value);
        }

        return value;
    }

    public void SetValue(string key, object value) => SetValue(EnvKey(key), value, _config.ExpireMinutes);

    public void SetValue(string key, object value, int expireMinutes)
    {
        Cache.StringSet(EnvKey(key), JsonSerializer.Serialize(value), TimeSpan.FromMinutes(expireMinutes));
    }

    public void SetValue<T>(string key, T value)
    {
        SetValue(EnvKey(key), (object)value!);
    }

    private string EnvKey(string key)
    {
        if (key.Contains(_envSufix))
        {
            return key;
        }

        return $"{key}_{_envSufix}";
    }
}
