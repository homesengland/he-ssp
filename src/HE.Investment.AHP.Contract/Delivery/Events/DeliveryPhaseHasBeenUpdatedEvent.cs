using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.Delivery.Events;

public record DeliveryPhaseHasBeenUpdatedEvent(string ApplicationId) : IDomainEvent;
