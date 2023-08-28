using System.Text.Json;
using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Services.Interfaces;
using StackExchange.Redis;

namespace HE.InvestmentLoans.Common.Services;

public class RedisService : ICacheService
{
    private readonly IAppConfig _appConfig;

    private readonly ConnectionMultiplexer _connection;

    public RedisService(IAppConfig appConfig, IDataverseConfig dataverseConfig)
    {
        _appConfig = appConfig;
        _connection = ConnectionMultiplexer.Connect(appConfig.Cache.RedisConnectionString);
    }

    private IDatabase Cache => _connection.GetDatabase();

    public T? GetValue<T>(string key)
    {
        if (Cache.KeyExists(GetGey(key)))
        {
            string? resp = Cache.StringGet(GetGey(key));
            return resp != null ? JsonSerializer.Deserialize<T>(resp) : default;
        }

        return default;
    }

    public async Task<T?> GetValueAsync<T>(string key, Func<Task<T>> loadValue)
    {
        if (Cache.KeyExists(GetGey(key)))
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
        Cache.StringSet(GetGey(key), JsonSerializer.Serialize(value), TimeSpan.FromMinutes(expireMinutes));
    }

    public void SetValue<T>(string key, T value)
    {
        SetValue(key, (object)value!);
    }

    private string GetGey(string key) => $"{_appConfig.AppName}_{key}";
}
