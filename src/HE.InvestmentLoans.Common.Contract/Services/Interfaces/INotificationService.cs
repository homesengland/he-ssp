using HE.InvestmentLoans.Common.Contract.Models;
using HE.InvestmentLoans.Common.Utils.Constants.Notification;

namespace HE.InvestmentLoans.Common.Contract.Services.Interfaces;
public interface INotificationService
{
    public void NotifySuccess(NotificationBodyType notificationBodyType, string valueToDisplay);

    public Tuple<bool, NotificationModel?> Pop();
}
