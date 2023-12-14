using FluentAssertions;
using HE.Investments.Account.Domain.Tests.UserOrganisation.TestDataBuilders;
using HE.Investments.Common.Exceptions;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.UserOrganisation.Entities.OrganisationUsersEntityTests;

public class InviteUserTests
{
    [Fact]
    public void ShouldThrowException_WhenUserIsAlreadyInOrganisation()
    {
        // given
        const string emailAddress = "test@email.com";

        var testCandidate = new OrganisationUsersEntityTestDataBuilder().WithActiveUser(emailAddress).Build();
        var invitation = new UserInvitationEntityTestDataBuilder().WithEmailAddress(emailAddress).Build();

        // when
        var invite = () => testCandidate.InviteUser(invitation);

        // then
        var errors = invite.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
        errors.Single().ErrorMessage.Should().Be("Enter a different email address, this email is already registered to a user in the organisation");
    }

    [Fact]
    public void ShouldThrowException_WhenUserIsAlreadyInvitedToOrganisation()
    {
        // given
        const string emailAddress = "test@email.com";

        var testCandidate = new OrganisationUsersEntityTestDataBuilder().WithPendingInvitation(emailAddress).Build();
        var invitation = new UserInvitationEntityTestDataBuilder().WithEmailAddress(emailAddress).Build();

        // when
        var invite = () => testCandidate.InviteUser(invitation);

        // then
        var errors = invite.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
        errors.Single().ErrorMessage.Should().Be("An invitation has already been sent to this email");
    }

    [Fact]
    public void ShouldThrowException_WhenUserIsInvitedTwice()
    {
        // given
        const string emailAddress = "test@email.com";

        var testCandidate = new OrganisationUsersEntityTestDataBuilder().Build();
        testCandidate.InviteUser(new UserInvitationEntityTestDataBuilder().WithEmailAddress(emailAddress).Build());

        var invitation = new UserInvitationEntityTestDataBuilder().WithEmailAddress(emailAddress).Build();

        // when
        var invite = () => testCandidate.InviteUser(invitation);

        // then
        var errors = invite.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
        errors.Single().ErrorMessage.Should().Be("An invitation has already been sent to this email");
    }

    [Fact]
    public void ShouldAddPendingInvitation_WhenUserDoesNotExistInOrganisation()
    {
        // given
        const string emailAddress = "test@email.com";

        var testCandidate = new OrganisationUsersEntityTestDataBuilder()
            .WithActiveUser("test1@email.com")
            .WithPendingInvitation("test2@email.com")
            .Build();
        var invitation = new UserInvitationEntityTestDataBuilder().WithEmailAddress(emailAddress).Build();

        // when
        testCandidate.InviteUser(invitation);

        // then
        testCandidate.PendingInvitations.Should().HaveCount(1).And.Contain(invitation);
    }
}
