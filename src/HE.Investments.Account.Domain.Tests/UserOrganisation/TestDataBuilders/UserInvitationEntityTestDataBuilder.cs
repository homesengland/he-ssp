using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Domain.UserOrganisation.Entities;
using HE.Investments.Account.Shared.User.ValueObjects;

namespace HE.Investments.Account.Domain.Tests.UserOrganisation.TestDataBuilders;

public class UserInvitationEntityTestDataBuilder
{
    private string _emailAddress = "test@test.com";

    public UserInvitationEntityTestDataBuilder WithEmailAddress(string emailAddress)
    {
        _emailAddress = emailAddress;
        return this;
    }

    public UserInvitationEntity Build()
    {
        return new UserInvitationEntity(
            new FirstName("John"),
            new LastName("Paul"),
            new EmailAddress(_emailAddress),
            new JobTitle("Engineer"),
            UserRole.ViewOnly);
    }
}
