using HE.Investments.Account.Domain.UserOrganisation.Entities;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Domain.Tests.UserOrganisation.TestDataBuilders;

public class OrganisationUsersEntityTestDataBuilder
{
    private readonly List<EmailAddress> _users = new();

    private readonly List<EmailAddress> _invitations = new();

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
        return new OrganisationUsersEntity(new OrganisationId(Guid.NewGuid().ToString()), _users, _invitations);
    }
}
