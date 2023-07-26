namespace HE.InvestmentLoans.Common.Services.Interfaces;

public interface ICacheService
{
    public T? GetValue<T>(string key);

    public T? GetValue<T>(string key, Func<T> loadValue);

    public Task<T?> GetValueAsync<T>(string key, Func<Task<T>> loadValue);

    public void SetValue<T>(string key, T value);

    public void SetValue(string key, object value);

    public void SetValue(string key, object value, int expireMinutes);
}
