using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investments.Account.Contract.User.Events;

public record UserAccountsChangedEvent(UserGlobalId UserGlobalId) : IDomainEvent;
