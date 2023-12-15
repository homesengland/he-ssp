using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Account.Domain.UserOrganisation.Entities;
using HE.Investments.Account.Shared.User.ValueObjects;

namespace HE.Investments.Account.Domain.Tests.UserOrganisation.TestDataBuilders;

public class OrganisationUsersEntityTestDataBuilder
{
    private readonly IList<EmailAddress> _users = new List<EmailAddress>();

    private readonly IList<EmailAddress> _invitations = new List<EmailAddress>();

    public OrganisationUsersEntityTestDataBuilder WithActiveUser(string emailAddress)
    {
        _users.Add(new EmailAddress(emailAddress));
        return this;
    }

    public OrganisationUsersEntityTestDataBuilder WithPendingInvitation(string emailAddress)
    {
        _invitations.Add(new EmailAddress(emailAddress));
        return this;
    }

    public OrganisationUsersEntity Build()
    {
        return new OrganisationUsersEntity(new OrganisationId(Guid.NewGuid()), _users, _invitations);
    }
}
