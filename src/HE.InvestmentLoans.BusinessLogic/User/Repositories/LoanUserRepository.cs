extern alias Org;

using System.Text.Json;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using HE.InvestmentLoans.CRM.Model;
using MediatR;
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

    public async Task<ContactRolesDto?> GetUserAccount(UserGlobalId userGlobalId, string userEmail)
    {
        const string portalType = "858110001";

        var req = new invln_getcontactroleRequest()
        {
            invln_contactemail = userEmail,
            invln_contactexternalid = userGlobalId.ToString(),
            invln_portaltype = portalType,
        };

        var resp_async = await _serviceClient.ExecuteAsync(req);
        var resp = resp_async != null ? (invln_getcontactroleResponse)resp_async : throw new NotFoundException("Contact role", userGlobalId);
        if (resp.invln_portalroles != null)
        {
            return CrmResponseSerializer.Deserialize<ContactRolesDto>(resp.invln_portalroles);
        }

        return null;
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
