using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.Delivery.Events;

public class DeliveryPhaseHasBeenUpdatedEvent : DomainEvent
{
    public DeliveryPhaseHasBeenUpdatedEvent(AhpApplicationId applicationId)
    {
        ApplicationId = applicationId;
    }

    public AhpApplicationId ApplicationId { get; }
}
