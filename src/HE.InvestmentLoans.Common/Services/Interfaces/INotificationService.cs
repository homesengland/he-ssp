namespace HE.InvestmentLoans.Common.Services.Interfaces;
public interface INotificationService
{
    public void Add(string key, string applicationName);

    public Tuple<bool, string> Pop(string key);
}
