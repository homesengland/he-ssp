using HE.InvestmentLoans.Common.Utils.Enums;

namespace HE.InvestmentLoans.Common.Utils.Constants.Notification;

public static class NotificationBodyLink
{
    public static string ContactEmailIfThereIsAProblem(IDictionary<NotificationServiceKeys, string> valuesToDisplay) =>
        new($"Contact {valuesToDisplay[NotificationServiceKeys.Email]} if you think there's a problem.");
}
