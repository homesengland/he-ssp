using HE.InvestmentLoans.Common.Contract.Models;
using HE.InvestmentLoans.Common.Utils.Constants.Notification;
using  HE.InvestmentLoans.Common.Utils.Enums;

namespace HE.InvestmentLoans.Common.Contract.Services.Interfaces;
public interface INotificationService
{
    public Task NotifySuccess(NotificationBodyType notificationBodyType, IDictionary<NotificationServiceKeys, string>? valuesToDisplay = null);
    public Tuple<bool, NotificationModel?> Pop();
}
