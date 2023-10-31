using HE.InvestmentLoans.Common.Utils.Constants.Notification;
using HE.InvestmentLoans.Common.Utils.Enums;

namespace HE.InvestmentLoans.WWW.Helpers.Notification;

public static class NotificationMapper
{
    public static string MapBodyTypeToDescription(NotificationBodyType bodyType, IDictionary<NotificationServiceKeys, string>? valuesToDisplay)
    {
        var text = bodyType switch
        {
            NotificationBodyType.WithdrawApplication => NotificationBody.ApplicationWithdrawnWithName,
            NotificationBodyType.DeleteProject => NotificationBody.ProjectRemoved,
            NotificationBodyType.FileRemove => NotificationBody.FileSuccesfullyRemoved,
            NotificationBodyType.FilesUpload => NotificationBody.FilesSuccesfullyUploaded,
            NotificationBodyType.ChangeOrganisationDetailsRequest => NotificationBody.ChangeOrganisationDetailsRequested,
            _ => string.Empty,
        };

        if (valuesToDisplay != null)
        {
            foreach (var value in valuesToDisplay)
            {
                text = text.Replace($"<{value.Key}>", value.Value);
            }
        }

        return text;
    }

    public static string MapBodyTypeToLinkDescription(NotificationBodyType bodyType, IDictionary<NotificationServiceKeys, string>? valuesToDisplay)
    {
        var text = bodyType switch
        {
            NotificationBodyType.WithdrawApplication => NotificationBodyLink.ContactEmailIfThereIsAProblem,
            NotificationBodyType.DeleteProject => string.Empty,
            _ => string.Empty,
        };

        if (valuesToDisplay != null)
        {
            foreach (var value in valuesToDisplay)
            {
                text = text.Replace($"<{value.Key}>", value.Value);
            }
        }

        return text;
    }
}
