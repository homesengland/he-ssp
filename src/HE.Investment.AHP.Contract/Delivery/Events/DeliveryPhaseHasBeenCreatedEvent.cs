using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.Delivery.Events;

public record DeliveryPhaseHasBeenCreatedEvent(AhpApplicationId ApplicationId, string DeliveryPhaseName) : IDomainEvent;
