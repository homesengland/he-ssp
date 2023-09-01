extern alias Org;

using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using Microsoft.PowerPlatform.Dataverse.Client;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;
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

    public async Task<ContactRolesDto?> GetUserAccount(UserGlobalId userGlobalId, string userEmail)
    {
        var contactRoles = await _contactService.GetContactRoles(_serviceClient, userEmail, PortalConstants.PortalType, userGlobalId.ToString());

        return contactRoles;
    }

    public Task LinkAccountToOrganization(UserGlobalId userGlobalId, CompaniesHouseNumber organizationNumber)
    {
        // Temporary dummy implementation
        return Task.CompletedTask;
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

        await _contactService.UpdateUserProfile(_serviceClient, userGlobalId.ToString(), contactDto);
    }
}
