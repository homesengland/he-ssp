using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investments.Account.Contract.Organisation.Events;

public record OrganisationHasBeenCreatedEvent(string OrganisationName) : IDomainEvent;
