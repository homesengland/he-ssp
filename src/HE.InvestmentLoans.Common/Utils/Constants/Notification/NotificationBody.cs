using HE.InvestmentLoans.Common.Utils.Enums;

namespace HE.InvestmentLoans.Common.Utils.Constants.Notification;

public static class NotificationBody
{
    public static string ApplicationWithdrawnWithName(IDictionary<NotificationServiceKeys, string> valuesToDisplay) =>
        new($"{valuesToDisplay[NotificationServiceKeys.Name]} project has been withdrawn.");

    public static string ProjectRemoved(IDictionary<NotificationServiceKeys, string> valuesToDisplay) =>
        new($"{valuesToDisplay[NotificationServiceKeys.Name]} removed");
}
