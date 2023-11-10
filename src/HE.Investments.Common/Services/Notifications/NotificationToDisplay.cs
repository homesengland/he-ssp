namespace HE.Investments.Common.Services.Notifications;

public record NotificationToDisplay(
    string Title,
    NotificationType NotificationType,
    string Description,
    string? LinkDescription = null,
    string? LinkDescriptionUrl = null,
    string? Body = null)
{
    public static NotificationToDisplay Success(
        string description,
        string? linkDescription = null,
        string? linkDescriptionUrl = null,
        string? body = null)
    {
        return new NotificationToDisplay(
            NotificationType.Success.ToString(),
            NotificationType.Success,
            description,
            linkDescription,
            linkDescriptionUrl,
            body);
    }
}
