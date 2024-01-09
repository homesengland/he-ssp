using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investments.Account.Contract.UserOrganisation.Events;

public record UserUnlinkedEvent(string UserGlobalId, string FirstName, string LastName) : DomainEvent;
