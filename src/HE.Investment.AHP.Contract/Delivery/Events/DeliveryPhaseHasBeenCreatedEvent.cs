using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.Delivery.Events;

public class DeliveryPhaseHasBeenCreatedEvent : DomainEvent
{
    public DeliveryPhaseHasBeenCreatedEvent(AhpApplicationId applicationId, string deliveryPhaseName)
    {
        ApplicationId = applicationId;
        DeliveryPhaseName = deliveryPhaseName;
    }

    public AhpApplicationId ApplicationId { get; }

    public string DeliveryPhaseName { get; }
}
