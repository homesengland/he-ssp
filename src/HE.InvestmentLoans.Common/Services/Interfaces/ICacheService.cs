namespace HE.InvestmentLoans.Common.Services.Interfaces;

public interface ICacheService
{
    public T? GetValue<T>(string key, Func<T> loadValue);
}
