namespace HE.InvestmentLoans.Common.Services.Interfaces;

public interface ICacheService
{
    public T? GetValue<T>(string key);

    public Task<T?> GetValueAsync<T>(string key, Func<Task<T>> loadValue);

    public void SetValue<T>(string key, T value);
}
