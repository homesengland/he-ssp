using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.Common.Exceptions;
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
        // TODO #65730: create correct parameters
        var contactRoles = await _contactService.GetContactRoles(_serviceClient, userEmail, userGlobalId.ToString(), 858110001); //last parameter to change

        if (contactRoles is null)
        {
            return Array.Empty<UserAccount>();
        }

        return contactRoles
            .contactRoles
            .GroupBy(x => x.accountId)
            .Select(x => new UserAccount(
                UserGlobalId.From(userGlobalId.ToString()),
                userEmail,
                x.Key,
                x.FirstOrDefault(y => y.accountId == x.Key)?.accountName ?? string.Empty,
                x.Select(x => new UserAccountRole(x.webRoleName)))).ToList();
    }

    public async Task<UserProfileDetails> GetProfileDetails(UserGlobalId userGlobalId)
    {
        var contactDto = await _contactService.RetrieveUserProfile(_serviceClient, userGlobalId.ToString())
                         ?? throw new NotFoundException(nameof(UserProfileDetails), userGlobalId.ToString());

        return new UserProfileDetails(
            new FirstName(contactDto.firstName),
            new LastName(contactDto.lastName),
            new JobTitle(contactDto.jobTitle),
            contactDto.email,
            new TelephoneNumber(contactDto.phoneNumber),
            contactDto.secondaryPhoneNumber.IsProvided() ? new SecondaryTelephoneNumber(contactDto.secondaryPhoneNumber) : null,
            contactDto.isTermsAndConditionsAccepted);
    }
}
