using HE.InvestmentLoans.Common.Utils.Constants.Notification;

namespace HE.InvestmentLoans.WWW.Helpers.Notification;

public static class NotificationMapper
{
    public static string MapBodyTypeToDescription(NotificationBodyType bodyType, string valueToDisplay)
    {
        return bodyType switch
        {
            NotificationBodyType.WithdrawApplication => NotificationBody.ApplicationWithdrawnWithName(valueToDisplay),
            NotificationBodyType.DeleteProject => NotificationBody.ProjectRemoved(valueToDisplay),
            _ => string.Empty,
        };
    }

    public static string MapBodyTypeToLinkDescription(NotificationBodyType bodyType)
    {
        return bodyType switch
        {
            NotificationBodyType.WithdrawApplication => NotificationBodyLink.ContactEmailIfThereIsAProblem,
            NotificationBodyType.DeleteProject => string.Empty,
            _ => string.Empty,
        };
    }
}
