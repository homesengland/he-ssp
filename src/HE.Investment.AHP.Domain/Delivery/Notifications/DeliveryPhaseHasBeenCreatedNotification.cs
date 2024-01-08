using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.Domain.Delivery.Notifications;

public class DeliveryPhaseHasBeenCreatedNotification : Notification
{
    public const string DeliveryPhaseNameParameterName = "DeliveryPhaseNameParameterName";

    public DeliveryPhaseHasBeenCreatedNotification(string deliveryPhaseName)
        : base(nameof(DeliveryPhaseHasBeenCreatedNotification), new Dictionary<string, string> { { DeliveryPhaseNameParameterName, deliveryPhaseName } })
    {
    }
}
