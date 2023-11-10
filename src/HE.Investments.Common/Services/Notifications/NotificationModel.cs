namespace HE.Investments.Common.Services.Notifications;

public record NotificationModel(string Title, NotificationType Type, NotificationBodyType NotificationBodyType,
    IDictionary<NotificationServiceKeys, string>? ValuesToDisplay);
