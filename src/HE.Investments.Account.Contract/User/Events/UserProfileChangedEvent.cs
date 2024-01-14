using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investments.Account.Contract.User.Events;

public class UserProfileChangedEvent : DomainEvent
{
    public UserProfileChangedEvent(UserGlobalId userGlobalId)
    {
        UserGlobalId = userGlobalId;
    }

    public UserGlobalId UserGlobalId { get; }
}
