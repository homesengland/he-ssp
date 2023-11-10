namespace HE.Investments.Common.Services.Notifications;

public record DisplayNotification(
    string Title,
    NotificationType NotificationType,
    string Description,
    string? LinkDescription = null,
    string? LinkDescriptionUrl = null,
    string? Body = null)
{
    public static DisplayNotification Success(
        string description,
        string? linkDescription = null,
        string? linkDescriptionUrl = null,
        string? body = null)
    {
        return new DisplayNotification(
            NotificationType.Success.ToString(),
            NotificationType.Success,
            description,
            linkDescription,
            linkDescriptionUrl,
            body);
    }
}
