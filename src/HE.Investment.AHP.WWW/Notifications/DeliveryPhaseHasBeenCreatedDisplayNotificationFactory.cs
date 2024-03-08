using HE.Investment.AHP.Domain.Delivery.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.WWW.Notifications;

public class DeliveryPhaseHasBeenCreatedDisplayNotificationFactory : IDisplayNotificationFactory
{
    public string HandledNotificationType => nameof(DeliveryPhaseHasBeenCreatedNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(
            notification.TemplateText($"<{DeliveryPhaseHasBeenCreatedNotification.DeliveryPhaseNameParameterName}> has been added to your scheme"));
    }
}
