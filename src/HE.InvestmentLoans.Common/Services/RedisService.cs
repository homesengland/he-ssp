using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Services.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace HE.InvestmentLoans.Common.Services;

public class RedisService : IRedisService
{
    private readonly IRedisConfig _config;
    private readonly ConnectionMultiplexer _connection;

    public RedisService(IRedisConfig config)
    {
        _config = config;
        _connection = ConnectionMultiplexer.Connect(config.ConnectionString);
    }

    private IDatabase Cache => _connection.GetDatabase();

    public bool Exists(string key) => Cache.KeyExists(key);

    public T? ObjectGet<T>(string key)
    {
        string? resp = Cache.StringGet(key);
        return resp != null ? JsonConvert.DeserializeObject<T>(resp) : default;
    }

    public bool ObjectSet(string key, object value)
    {
        return Cache.StringSet(key, JsonConvert.SerializeObject(value), TimeSpan.FromMinutes(_config.ExpireMinutes));
    }

    public bool ObjectSet(string key, object value, int expireMinutes)
    {
        return Cache.StringSet(key, JsonConvert.SerializeObject(value), TimeSpan.FromMinutes(expireMinutes));
    }
}
