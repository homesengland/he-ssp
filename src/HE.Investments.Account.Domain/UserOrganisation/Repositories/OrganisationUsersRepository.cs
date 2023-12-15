using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Domain.Data;
using HE.Investments.Account.Domain.Data.Extensions;
using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Account.Domain.UserOrganisation.Entities;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Organisation.Services;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Account.Domain.UserOrganisation.Repositories;

public class OrganisationUsersRepository : IOrganisationUsersRepository
{
    private readonly IOrganizationServiceAsync2 _organizationServiceAsync;

    private readonly IContactService _contactService;

    private readonly IAccountUserContext _userContext;

    public OrganisationUsersRepository(IOrganizationServiceAsync2 organizationServiceAsync, IContactService contactService, IAccountUserContext userContext)
    {
        _organizationServiceAsync = organizationServiceAsync;
        _contactService = contactService;
        _userContext = userContext;
    }

    public async Task<OrganisationUsersEntity> GetOrganisationUsers(OrganisationId organisationId, CancellationToken cancellationToken)
    {
        var contacts = await _contactService.GetAllOrganisationContactsForPortal(_organizationServiceAsync, organisationId.Value);
        var activeUsers = contacts.Where(x => x.IsConnectedWithExternalIdentity()).Select(x => new EmailAddress(x.email));
        var invitedUsers = contacts.Where(x => !x.IsConnectedWithExternalIdentity()).Select(x => new EmailAddress(x.email));

        return new OrganisationUsersEntity(organisationId, activeUsers, invitedUsers);
    }

    public async Task Save(OrganisationUsersEntity organisationUsers, CancellationToken cancellationToken)
    {
        var invitationsToSend = organisationUsers.PendingInvitations.ToList();
        foreach (var invitation in invitationsToSend)
        {
            await _contactService.CreateNotConnectedContact(
                _organizationServiceAsync,
                new ContactDto
                {
                    email = invitation.Email.Value,
                    firstName = invitation.FirstName.Value,
                    lastName = invitation.LastName.Value,
                    jobTitle = invitation.JobTitle.Value,
                },
                organisationUsers.OrganisationId.Value,
                UserRoleMapper.ToDto(invitation.Role)!.Value,
                _userContext.UserGlobalId.ToString());

            organisationUsers.InvitationSent(invitation);
        }
    }
}
