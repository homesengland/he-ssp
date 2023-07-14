namespace HE.InvestmentLoans.Common.Services.Interfaces;

public interface IRedisService
{
    public bool Exists(string key);

    public T? ObjectGet<T>(string key);

    public bool ObjectSet(string key, object value);

    public bool ObjectSet(string key, object value, int expireMinutes);
}
