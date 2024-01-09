using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.HomeTypes.Events;

public record HomeTypeHasBeenUpdatedEvent(string ApplicationId, string HomeTypeId) : DomainEvent;
