namespace HE.Investments.Common.Services.Notifications;

public interface INotificationKeyFactory
{
    string CreateKey(ApplicationType application);
}
