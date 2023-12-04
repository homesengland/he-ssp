using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.Application.Events;

public record ApplicationHasBeenCreatedEvent(string ApplicationId) : DomainEvent;
