namespace HE.Investments.Common.Infrastructure.Cache.Interfaces;

public interface ICacheService
{
    public T? GetValue<T>(string key);

    public Task<T?> GetValueAsync<T>(string key, Func<Task<T>> loadValue);

    public void SetValue<T>(string key, T value);

    public Task SetValueAsync<T>(string key, T value);
}
