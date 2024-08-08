namespace HE.UtilsService.BannerNotification.Shared;

public class NotificationKeyFactory : INotificationKeyFactory
{
    public string KeyForOrganisation(string organisationId, ApplicationType application, ApplicationArea? area)
    {
        if (area.GetValueOrDefault() == ApplicationArea.Undefined)
        {
            return $"notification-{application.ToString().ToLowerInvariant()}-{organisationId}";
        }

        return $"notification-{application.ToString().ToLowerInvariant()}-{area.ToString()!.ToLowerInvariant()}-{organisationId}";
    }

    public string KeyForUser(string userGlobalId, ApplicationType application)
    {
        return $"notification-{application.ToString().ToLowerInvariant()}-{userGlobalId}";
    }
}
