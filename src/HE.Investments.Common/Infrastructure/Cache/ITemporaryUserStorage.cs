namespace HE.Investments.Common.Infrastructure.Cache;

public interface ITemporaryUserStorage
{
    T? GetValue<T>(string key);

    void SetValue<T>(string key, T value);

    void Delete(string key);
}
