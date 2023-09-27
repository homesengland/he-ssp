using HE.InvestmentLoans.Common.Contract.Models;

namespace HE.InvestmentLoans.Common.Contract.Services.Interfaces;
public interface INotificationService
{
    public void NotifySuccess(string notificationBody, bool displayBodyLink);

    public Tuple<bool, NotificationModel?> Pop();
}
