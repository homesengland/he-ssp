using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.Delivery.Events;

public record DeliveryPhaseHasBeenCreatedEvent(string ApplicationId, string DeliveryPhaseName) : DomainEvent;
