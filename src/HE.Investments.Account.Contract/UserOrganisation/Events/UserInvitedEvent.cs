using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investments.Account.Contract.UserOrganisation.Events;

public record UserInvitedEvent(string FirstName, string LastName) : IDomainEvent;
