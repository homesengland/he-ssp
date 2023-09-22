using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Services.Interfaces;
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
            return resp != null ? JsonSerializer.Deserialize<T>(resp) : default;
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
        Cache.StringSet(GetKey(key), JsonSerializer.Serialize(value), TimeSpan.FromMinutes(expireMinutes));
    }

    public void SetValue<T>(string key, T value)
    {
        SetValue(key, (object)value!);
    }

    private string GetKey(string key) => $"{_appConfig.AppName}_{key}";
}
