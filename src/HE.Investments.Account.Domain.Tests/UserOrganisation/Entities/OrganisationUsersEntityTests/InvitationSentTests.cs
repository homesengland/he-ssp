using FluentAssertions;
using HE.Investments.Account.Domain.Tests.UserOrganisation.TestDataBuilders;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.UserOrganisation.Entities.OrganisationUsersEntityTests;

public class InvitationSentTests
{
    [Fact]
    public void ShouldThrowException_WhenInvitationDoesNotExist()
    {
        // given
        const string emailAddress = "test@email.com";

        var testCandidate = new OrganisationUsersEntityTestDataBuilder().Build();
        var invitation = new UserInvitationEntityTestDataBuilder().WithEmailAddress(emailAddress).Build();

        // when
        var invitationSent = () => testCandidate.InvitationSent(invitation);

        // then
        invitationSent.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ShouldRemovePendingInvitation_WhenInvitationExists()
    {
        // given
        const string emailAddress = "test@email.com";

        var testCandidate = new OrganisationUsersEntityTestDataBuilder().Build();
        var invitation = new UserInvitationEntityTestDataBuilder().WithEmailAddress(emailAddress).Build();
        testCandidate.InviteUser(invitation);

        // when
        testCandidate.InvitationSent(invitation);

        // then
        testCandidate.PendingInvitations.Should().BeEmpty();
    }
}
