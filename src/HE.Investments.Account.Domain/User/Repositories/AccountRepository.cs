using System.Collections.ObjectModel;
using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Contract.User.Events;
using HE.Investments.Account.Domain.User.Repositories.Mappers;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Repositories;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Organisation.Services;
using MediatR;
using Microsoft.PowerPlatform.Dataverse.Client;
using UserAccount = HE.Investments.Account.Shared.User.UserAccount;

namespace HE.Investments.Account.Domain.User.Repositories;

public class AccountRepository : IProfileRepository, IAccountRepository
{
    private readonly IContactService _contactService;

    private readonly IOrganizationServiceAsync2 _serviceClient;

    private readonly IMediator _mediator;

    public AccountRepository(IContactService contactService, IOrganizationServiceAsync2 serviceClient, IMediator mediator)
    {
        _contactService = contactService;
        _serviceClient = serviceClient;
        _mediator = mediator;
    }

    public async Task<IList<UserAccount>> GetUserAccounts(UserGlobalId userGlobalId, string userEmail)
    {
        var contactExternalId = userGlobalId.ToString();
        var contactRoles = await _contactService.GetContactRoles(_serviceClient, userEmail, contactExternalId);
        if (contactRoles is null)
        {
            return Array.Empty<UserAccount>();
        }

        // TODO: #88197 - Fetch IsUnregisteredBody
        return contactRoles
            .contactRoles
            .GroupBy(x => x.accountId)
            .Select(x => new UserAccount(
                UserGlobalId.From(contactExternalId),
                userEmail,
                new OrganisationBasicInfo(new OrganisationId(x.Key), false),
                x.FirstOrDefault(y => y.accountId == x.Key)?.accountName ?? string.Empty,
                new ReadOnlyCollection<UserRole>(x.Where(y => y.permission.HasValue).Select(y => UserRoleMapper.ToDomain(y.permission)!.Value).ToList())))
            .ToList();
    }

    public async Task<UserProfileDetails> GetProfileDetails(UserGlobalId userGlobalId)
    {
        var contactDto = await _contactService.RetrieveUserProfile(_serviceClient, userGlobalId.ToString())
                         ?? throw new NotFoundException(nameof(UserProfileDetails), userGlobalId.ToString());

        var userDetails = UserProfileMapper.MapContactDtoToUserDetails(contactDto);

        return userDetails;
    }

    public async Task SaveAsync(UserProfileDetails userProfileDetails, UserGlobalId userGlobalId, CancellationToken cancellationToken)
    {
        var contactDto = UserProfileMapper.MapUserDetailsToContactDto(userProfileDetails);

        await _contactService.UpdateUserProfile(_serviceClient, userGlobalId.ToString(), contactDto, cancellationToken);
        await _mediator.Publish(new UserProfileChangedEvent(userGlobalId), cancellationToken);
    }
}
