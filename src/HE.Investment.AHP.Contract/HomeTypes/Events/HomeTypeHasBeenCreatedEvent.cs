using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.HomeTypes.Events;

public record HomeTypeHasBeenCreatedEvent(string ApplicationId, string HomeTypeId, string HomeTypeName) : DomainEvent;
