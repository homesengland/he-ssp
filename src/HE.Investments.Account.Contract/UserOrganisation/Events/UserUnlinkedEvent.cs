using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investments.Account.Contract.UserOrganisation.Events;

public record UserUnlinkedEvent(string FirstName, string LastName) : DomainEvent;
