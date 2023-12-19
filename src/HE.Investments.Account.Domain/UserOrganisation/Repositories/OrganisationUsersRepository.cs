using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Domain.Data;
using HE.Investments.Account.Domain.Data.Extensions;
using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Account.Domain.UserOrganisation.Entities;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Organisation.Services;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Account.Domain.UserOrganisation.Repositories;

public class OrganisationUsersRepository : IOrganisationUsersRepository
{
    private readonly IOrganizationServiceAsync2 _organizationServiceAsync;

    private readonly IContactService _contactService;

    private readonly IAccountUserContext _userContext;

    private readonly IEventDispatcher _eventDispatcher;

    public OrganisationUsersRepository(IOrganizationServiceAsync2 organizationServiceAsync, IContactService contactService, IAccountUserContext userContext, IEventDispatcher eventDispatcher)
    {
        _organizationServiceAsync = organizationServiceAsync;
        _contactService = contactService;
        _userContext = userContext;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<OrganisationUsersEntity> GetOrganisationUsers(OrganisationId organisationId, CancellationToken cancellationToken)
    {
        var filter = new ContactService.ContactFilters(10, 1, organisationId.Value, new List<int>
        {
            UserRoleMapper.ToDto(UserRole.Admin)!.Value,
            UserRoleMapper.ToDto(UserRole.Enhanced)!.Value,
            UserRoleMapper.ToDto(UserRole.Input)!.Value,
            UserRoleMapper.ToDto(UserRole.ViewOnly)!.Value,
            UserRoleMapper.ToDto(UserRole.Limited)!.Value,
        });
        var contacts = await _contactService.GetAllOrganisationContactsForPortal(_organizationServiceAsync, filter);
        var activeUsers = contacts.Items.Where(x => x.IsConnectedWithExternalIdentity()).Select(x => new EmailAddress(x.email));
        var invitedUsers = contacts.Items.Where(x => !x.IsConnectedWithExternalIdentity()).Select(x => new EmailAddress(x.email));

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

        await DispatchEvents(organisationUsers, cancellationToken);
    }

    private async Task DispatchEvents(DomainEntity domainEntity, CancellationToken cancellationToken)
    {
        await _eventDispatcher.Publish(domainEntity, cancellationToken);
    }
}
