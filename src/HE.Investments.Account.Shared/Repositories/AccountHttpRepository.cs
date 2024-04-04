using System.Net.Http.Json;
using System.Web;
using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Extensions;

namespace HE.Investments.Account.Shared.Repositories;

internal class AccountHttpRepository : IAccountRepository
{
    private readonly HttpClient _httpClient;

    public AccountHttpRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IList<UserAccount>> GetUserAccounts(UserGlobalId userGlobalId, string userEmail)
    {
        var accounts = await _httpClient.GetFromJsonAsync<AccountDetails>($"api/user/{userGlobalId}/accounts?userEmail={HttpUtility.UrlEncode(userEmail)}");
        if (accounts == null)
        {
            throw new NotFoundException($"Cannot find User Account with id {userGlobalId}.");
        }

        return accounts.Organisations.Select(x => new UserAccount(
                userGlobalId,
                userEmail,
                new OrganisationBasicInfo(
                    new OrganisationId(x.Organisation.OrganisationId),
                    x.Organisation.CompanyRegisteredName,
                    x.Organisation.CompanyRegistrationNumber,
                    x.Organisation.CompanyAddressLine1,
                    x.Organisation.IsUnregisteredBody),
                x.Roles))
            .ToList();
    }

    public async Task<UserProfileDetails> GetProfileDetails(UserGlobalId userGlobalId)
    {
        var profile = await _httpClient.GetFromJsonAsync<ProfileDetails>($"api/user/{userGlobalId}/profile");
        if (profile == null)
        {
            throw new NotFoundException($"Cannot find User Profile with id {userGlobalId}.");
        }

        return new UserProfileDetails(
            profile.FirstName.IsProvided() ? new YourFirstName(profile.FirstName) : null,
            profile.LastName.IsProvided() ? new YourLastName(profile.LastName) : null,
            profile.JobTitle.IsProvided() ? new YourJobTitle(profile.JobTitle) : null,
            profile.Email,
            profile.TelephoneNumber.IsProvided() ? new TelephoneNumber(profile.TelephoneNumber!) : null,
            profile.SecondaryTelephoneNumber.IsProvided() ? new TelephoneNumber(profile.SecondaryTelephoneNumber!) : null,
            profile.IsTermsAndConditionsAccepted);
    }
}
