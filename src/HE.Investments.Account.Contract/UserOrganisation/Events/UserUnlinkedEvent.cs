using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investments.Account.Contract.UserOrganisation.Events;

public class UserUnlinkedEvent : DomainEvent
{
    public UserUnlinkedEvent(UserGlobalId userGlobalId, string firstName, string lastName)
    {
        UserGlobalId = userGlobalId;
        FirstName = firstName;
        LastName = lastName;
    }

    public UserGlobalId UserGlobalId { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }
}
