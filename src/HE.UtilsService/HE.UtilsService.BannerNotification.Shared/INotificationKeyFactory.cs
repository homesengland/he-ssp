namespace HE.UtilsService.BannerNotification.Shared;

public interface INotificationKeyFactory
{
    string KeyForOrganisation(string organisationId, ApplicationType application, ApplicationArea? area);
}
