namespace HE.InvestmentLoans.Common.Services.Interfaces;

public interface ICacheService
{
    public T? GetValue<T>(string key, Func<T> loadValue);

    public Task<T?> GetValueAsync<T>(string key, Func<Task<T>> loadValue);
}
