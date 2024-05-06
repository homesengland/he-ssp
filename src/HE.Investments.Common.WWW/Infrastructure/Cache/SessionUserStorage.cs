using System.Text.Json;
using HE.Investments.Common.Infrastructure.Cache;
using Microsoft.AspNetCore.Http;

namespace HE.Investments.Common.WWW.Infrastructure.Cache;

public class SessionUserStorage : ITemporaryUserStorage
{
    private readonly ISession _session;

    public SessionUserStorage(ISession session)
    {
        _session = session;
    }

    public T? GetValue<T>(string key)
    {
        var value = _session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }

    public void SetValue<T>(string key, T value)
    {
        _session.SetString(key, JsonSerializer.Serialize(value));
    }

    public void Delete(string key)
    {
        _session.Remove(key);
    }
}
