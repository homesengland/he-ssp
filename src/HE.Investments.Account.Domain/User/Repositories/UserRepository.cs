using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.Investments.Account.Domain.User.Entities;
using HE.Investments.Account.Domain.User.Repositories.Mappers;
using HE.Investments.Account.Shared.User;
using HE.Investments.Organisation.Services;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Account.Domain.User.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IContactService _contactService;

    private readonly IOrganizationServiceAsync2 _serviceClient;

    public UserRepository(IContactService contactService, IOrganizationServiceAsync2 serviceClient)
    {
        _contactService = contactService;
        _serviceClient = serviceClient;
    }

    public async Task<IList<UserAccount>> GetUserAccounts(UserGlobalId userGlobalId, string userEmail)
    {
        var contactRoles = await _contactService.GetContactRoles(_serviceClient, userEmail, PortalConstants.LoansPortalType, userGlobalId.ToString());

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

    public async Task<UserProfileDetails> GetUserProfileInformation(UserGlobalId userGlobalId)
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
    }
}
