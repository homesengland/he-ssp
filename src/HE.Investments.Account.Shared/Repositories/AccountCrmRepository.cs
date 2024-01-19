using System.Collections.ObjectModel;
using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Extensions;
using HE.Investments.Organisation.Services;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Account.Shared.Repositories;

public class AccountCrmRepository : IAccountRepository
{
    private readonly IContactService _contactService;

    private readonly IOrganizationServiceAsync2 _serviceClient;

    public AccountCrmRepository(IContactService contactService, IOrganizationServiceAsync2 serviceClient)
    {
        _contactService = contactService;
        _serviceClient = serviceClient;
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

        return new UserProfileDetails(
            contactDto.firstName.IsProvided() ? new FirstName(contactDto.firstName) : null,
            contactDto.lastName.IsProvided() ? new LastName(contactDto.lastName) : null,
            contactDto.jobTitle.IsProvided() ? new JobTitle(contactDto.jobTitle) : null,
            contactDto.email,
            contactDto.phoneNumber.IsProvided() ? new TelephoneNumber(contactDto.phoneNumber) : null,
            contactDto.secondaryPhoneNumber.IsProvided() ? new SecondaryTelephoneNumber(contactDto.secondaryPhoneNumber) : null,
            contactDto.isTermsAndConditionsAccepted);
    }
}
