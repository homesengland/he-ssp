using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investments.Account.Contract.User.Events;

public class UserAccountsChangedEvent : DomainEvent
{
    public UserAccountsChangedEvent(UserGlobalId userGlobalId)
    {
        UserGlobalId = userGlobalId;
    }

    public UserGlobalId UserGlobalId { get; private set; }
}
