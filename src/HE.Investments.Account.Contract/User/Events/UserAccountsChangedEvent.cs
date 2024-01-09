using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investments.Account.Contract.User.Events;

public record UserAccountsChangedEvent(string UserGlobalId) : DomainEvent;
