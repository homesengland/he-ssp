using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.HomeTypes.Events;

public record HomeTypeHasBeenUpdatedEvent(string ApplicationId, string HomeTypeId) : DomainEvent;
