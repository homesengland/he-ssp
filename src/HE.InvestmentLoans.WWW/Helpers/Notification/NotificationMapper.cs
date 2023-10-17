using HE.InvestmentLoans.Common.Utils.Constants.Notification;
using HE.InvestmentLoans.Common.Utils.Enums;

namespace HE.InvestmentLoans.WWW.Helpers.Notification;

public static class NotificationMapper
{
    public static string MapBodyTypeToDescription(NotificationBodyType bodyType, IDictionary<NotificationServiceKeys, string> valuesToDisplay)
    {
        return bodyType switch
        {
            NotificationBodyType.WithdrawApplication => NotificationBody.ApplicationWithdrawnWithName(valuesToDisplay),
            NotificationBodyType.DeleteProject => NotificationBody.ProjectRemoved(valuesToDisplay),
            _ => string.Empty,
        };
    }

    public static string MapBodyTypeToLinkDescription(NotificationBodyType bodyType, IDictionary<NotificationServiceKeys, string> valuesToDisplay)
    {
        return bodyType switch
        {
            NotificationBodyType.WithdrawApplication => NotificationBodyLink.ContactEmailIfThereIsAProblem(valuesToDisplay),
            NotificationBodyType.DeleteProject => string.Empty,
            _ => string.Empty,
        };
    }
}
