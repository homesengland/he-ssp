using System.Collections.ObjectModel;
using HE.Investments.Account.Api.Contract.User;
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

    private readonly IOrganizationService _organizationService;

    private readonly IOrganizationServiceAsync2 _serviceClient;

    public AccountCrmRepository(IContactService contactService, IOrganizationService organizationService, IOrganizationServiceAsync2 serviceClient)
    {
        _contactService = contactService;
        _serviceClient = serviceClient;
        _organizationService = organizationService;
    }

    public async Task<IList<UserAccount>> GetUserAccounts(UserGlobalId userGlobalId, string userEmail)
    {
        var contactExternalId = userGlobalId.ToString();
        var contactRoles = await _contactService.GetContactRoles(_serviceClient, userEmail, contactExternalId);
        if (contactRoles is null)
        {
            return Array.Empty<UserAccount>();
        }

        var result = new List<UserAccount>();
        foreach (var contactRole in contactRoles.contactRoles.GroupBy(x => x.accountId))
        {
            var organisationId = OrganisationId.From(contactRole.Key.ToString());

            // TODO: replace loop with single request when user will be assigned to multiple organisations
            var organisation = await GetOrganisationBasicInfo(organisationId, userGlobalId);
            var userAccount = new UserAccount(
                UserGlobalId.From(contactExternalId),
                userEmail,
                organisation,
                new ReadOnlyCollection<UserRole>(contactRole.Where(y => y.permission.HasValue)
                    .Select(y => UserRoleMapper.ToDomain(y.permission)!.Value)
                    .ToList()));

            result.Add(userAccount);
        }

        return result;
    }

    public async Task<UserProfileDetails> GetProfileDetails(UserGlobalId userGlobalId)
    {
        var contactDto = await _contactService.RetrieveUserProfile(_serviceClient, userGlobalId.ToString())
                         ?? throw new NotFoundException(nameof(UserProfileDetails), userGlobalId.ToString());

        return new UserProfileDetails(
            contactDto.firstName.IsProvided() ? new YourFirstName(contactDto.firstName) : null,
            contactDto.lastName.IsProvided() ? new YourLastName(contactDto.lastName) : null,
            contactDto.jobTitle.IsProvided() ? new YourJobTitle(contactDto.jobTitle) : null,
            contactDto.email,
            contactDto.phoneNumber.IsProvided() ? new TelephoneNumber(contactDto.phoneNumber) : null,
            contactDto.secondaryPhoneNumber.IsProvided() ? new TelephoneNumber(contactDto.secondaryPhoneNumber) : null,
            contactDto.isTermsAndConditionsAccepted);
    }

    private async Task<OrganisationBasicInfo> GetOrganisationBasicInfo(OrganisationId organisationId, UserGlobalId userGlobalId)
    {
        var organisation = await _organizationService.GetOrganizationDetails(organisationId.Value, userGlobalId.Value);
        return new OrganisationBasicInfo(
            organisationId,
            organisation.registeredCompanyName,
            organisation.companyRegistrationNumber,
            organisation.addressLine1,
            organisation.isUnregisteredBody);
    }
}
