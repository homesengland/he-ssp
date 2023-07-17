namespace HE.InvestmentLoans.Common.Services.Interfaces;

public interface ICacheService
{
    public bool Exists(string key);

    public T? ObjectGet<T>(string key);

    public bool ObjectSet(string key, object value);

    public bool ObjectSet(string key, object value, int expireMinutes);
}
