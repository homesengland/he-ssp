using HE.Investments.Account.Contract.UserOrganisation.Events;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;

namespace HE.Investments.Account.Domain.UserOrganisation.Entities;

public class OrganisationUsersEntity : DomainEntity
{
    private readonly IReadOnlyCollection<EmailAddress> _activeUserEmails;

    private readonly IList<EmailAddress> _invitedUserEmails;

    private readonly IList<UserInvitationEntity> _pendingInvitations = new List<UserInvitationEntity>();

    public OrganisationUsersEntity(
        OrganisationId organisationId,
        IEnumerable<EmailAddress> activeUsers,
        IEnumerable<EmailAddress> invitedUsers)
    {
        OrganisationId = organisationId;
        _activeUserEmails = activeUsers.ToList();
        _invitedUserEmails = invitedUsers.ToList();
    }

    public OrganisationId OrganisationId { get; }

    public IEnumerable<EmailAddress> ActiveUsers => _activeUserEmails;

    public IEnumerable<EmailAddress> InvitedUsers => _invitedUserEmails;

    public IEnumerable<UserInvitationEntity> PendingInvitations => _pendingInvitations;

    public void InviteUser(UserInvitationEntity invitation)
    {
        if (_activeUserEmails.Any(x => x == invitation.Email))
        {
            OperationResult.New()
                .AddValidationError(nameof(UserInvitationEntity.Email), "Enter a different email address, this email is already registered to a user in the organisation")
                .CheckErrors();
        }

        if (_invitedUserEmails.Any(x => x == invitation.Email)
            || _pendingInvitations.Any(x => x.Email == invitation.Email))
        {
            OperationResult.New()
                .AddValidationError(nameof(UserInvitationEntity.Email), "An invitation has already been sent to this email")
                .CheckErrors();
        }

        _pendingInvitations.Add(invitation);
        Publish(new UserInvitedEvent(invitation.FirstName.Value, invitation.LastName.Value));
    }

    public void InvitationSent(UserInvitationEntity pendingInvitation)
    {
        if (!_pendingInvitations.Contains(pendingInvitation))
        {
            throw new InvalidOperationException("Cannot mark Invitation as sent because it does not belong to current entity.");
        }

        _pendingInvitations.Remove(pendingInvitation);
        _invitedUserEmails.Add(pendingInvitation.Email);
    }
}
