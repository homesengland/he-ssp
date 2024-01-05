using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investments.Account.Contract.User.Events;

public record UserAccountsChangedEvent(string UserGlobalId) : IDomainEvent;
