using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Services.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace HE.InvestmentLoans.Common.Services;

public class RedisService : ICacheService
{
    private readonly IAppConfig _appConfig;

    private readonly ConnectionMultiplexer _connection;

    public RedisService(IAppConfig appConfig, ConfigurationOptions options)
    {
        _appConfig = appConfig;

        _connection = ConnectionMultiplexer.Connect(options);
    }

    private IDatabase Cache => _connection.GetDatabase();

    public T? GetValue<T>(string key)
    {
        if (Cache.KeyExists(GetKey(key)))
        {
            string? resp = Cache.StringGet(GetKey(key));
            return resp != null ? JsonConvert.DeserializeObject<T>(resp) : default;
        }

        return default;
    }

    public async Task<T?> GetValueAsync<T>(string key, Func<Task<T>> loadValue)
    {
        if (Cache.KeyExists(GetKey(key)))
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

    public void SetValue(string key, object value) => SetValue(key, value, _appConfig.Cache.ExpireMinutes);

    public void SetValue(string key, object value, int expireMinutes)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
        };

        var serializedValue = JsonConvert.SerializeObject(value, settings);
        Cache.StringSet(GetKey(key), serializedValue, TimeSpan.FromMinutes(expireMinutes));
    }

    public void SetValue<T>(string key, T value)
    {
        SetValue(key, (object)value!);
    }

    private string GetKey(string key) => $"{_appConfig.AppName}_{key}";
}
