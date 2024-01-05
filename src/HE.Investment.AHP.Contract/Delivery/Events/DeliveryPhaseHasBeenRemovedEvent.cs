using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.Delivery.Events;

public record DeliveryPhaseHasBeenRemovedEvent(string ApplicationId) : DomainEvent;
