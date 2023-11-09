extern alias Org;

using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.Investments.Account.Shared.User;
using Microsoft.PowerPlatform.Dataverse.Client;
using Org::HE.Investments.Organisation.Services;

namespace HE.InvestmentLoans.BusinessLogic.User.Repositories;

public class LoanUserRepository : ILoanUserRepository
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    private readonly IContactService _contactService;

    public LoanUserRepository(IOrganizationServiceAsync2 serviceClient, IContactService contactService)
    {
        _serviceClient = serviceClient;
        _contactService = contactService;
    }

    public async Task<UserDetails> GetUserDetails(UserGlobalId userGlobalId)
    {
        var contactDto = await _contactService.RetrieveUserProfile(_serviceClient, userGlobalId.ToString())
            ?? throw new NotFoundException(nameof(UserDetails), userGlobalId.ToString());

        var userDetails = UserDetailsMapper.MapContactDtoToUserDetails(contactDto);

        return userDetails;
    }

    public async Task SaveAsync(UserDetails userDetails, UserGlobalId userGlobalId, CancellationToken cancellationToken)
    {
        var contactDto = UserDetailsMapper.MapUserDetailsToContactDto(userDetails);

        await _contactService.UpdateUserProfile(_serviceClient, userGlobalId.ToString(), contactDto, cancellationToken);
    }
}
