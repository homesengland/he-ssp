using HE.InvestmentLoans.Common.Utils.Constants.Notification;
using HE.InvestmentLoans.Common.Utils.Enums;

namespace HE.InvestmentLoans.Common.Contract.Models;

public record NotificationModel(string Title, NotificationType Type, NotificationBodyType NotificationBodyType,
    IDictionary<NotificationServiceKeys, string>? ValuesToDisplay);
