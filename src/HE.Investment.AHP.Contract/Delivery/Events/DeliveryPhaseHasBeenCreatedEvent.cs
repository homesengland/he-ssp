using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.Delivery.Events;

public record DeliveryPhaseHasBeenCreatedEvent(string ApplicationId, string DeliveryPhaseName) : DomainEvent;
