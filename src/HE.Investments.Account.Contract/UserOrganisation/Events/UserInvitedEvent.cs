using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investments.Account.Contract.UserOrganisation.Events;

public class UserInvitedEvent : DomainEvent
{
    public UserInvitedEvent(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }
}
