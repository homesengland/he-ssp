using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.Delivery.Events;

public class DeliveryPhaseHasBeenRemovedEvent : DomainEvent
{
    public DeliveryPhaseHasBeenRemovedEvent(AhpApplicationId applicationId)
    {
        ApplicationId = applicationId;
    }

    public AhpApplicationId ApplicationId { get; }
}
