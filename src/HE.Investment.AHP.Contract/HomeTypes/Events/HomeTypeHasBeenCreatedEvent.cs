using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.HomeTypes.Events;

public record HomeTypeHasBeenCreatedEvent(string? HomeTypeName) : DomainEvent;
