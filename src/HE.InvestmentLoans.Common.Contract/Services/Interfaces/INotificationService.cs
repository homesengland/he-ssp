using HE.InvestmentLoans.Common.Contract.Models;
using HE.InvestmentLoans.Common.Utils.Constants.Notification;
using  HE.InvestmentLoans.Common.Utils.Enums;

namespace HE.InvestmentLoans.Common.Contract.Services.Interfaces;
public interface INotificationService
{
    public void NotifySuccess(NotificationBodyType notificationBodyType, IDictionary<NotificationServiceKeys, string> valuesToDisplay);

    public Tuple<bool, NotificationModel?> Pop();
}
