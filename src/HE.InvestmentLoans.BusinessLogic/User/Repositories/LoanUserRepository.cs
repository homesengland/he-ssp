using System.Text.Json;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using HE.InvestmentLoans.CRM.Model;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.InvestmentLoans.BusinessLogic.User.Repositories;

public class LoanUserRepository : ILoanUserRepository
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    public LoanUserRepository(IOrganizationServiceAsync2 serviceClient)
    {
        _serviceClient = serviceClient;
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
        var req = new invln_returnuserprofileRequest()
        {
            invln_contactexternalid = userGlobalId.ToString(),
        };

        var resp_async = await _serviceClient.ExecuteAsync(req);
        var resp = resp_async != null ? (invln_returnuserprofileResponse)resp_async : throw new NotFoundException(nameof(UserDetails), userGlobalId.ToString());

        var contactDto = CrmResponseSerializer.Deserialize<ContactDto>(resp.invln_userprofile) ?? throw new NotFoundException(nameof(UserDetails), userGlobalId.ToString());

        return new UserDetails(
            contactDto.firstName,
            contactDto.lastName,
            contactDto.jobTitle,
            contactDto.email,
            contactDto.phoneNumber,
            contactDto.secondaryPhoneNumber,
            contactDto.isTermsAndConditionsAccepted);
    }

    public async Task SaveAsync(UserDetails userDetails, UserGlobalId userGlobalId, CancellationToken cancellationToken)
    {
        var contactDto = new ContactDto
        {
            firstName = userDetails.FirstName,
            lastName = userDetails.Surname,
            jobTitle = userDetails.JobTitle,
            email = userDetails.Email,
            phoneNumber = userDetails.TelephoneNumber,
            secondaryPhoneNumber = userDetails.SecondaryTelephoneNumber,
            isTermsAndConditionsAccepted = userDetails.IsTermsAndConditionsAccepted,
        };

        var contactDtoSerialized = JsonSerializer.Serialize(contactDto);

        var req = new invln_updateuserprofileRequest
        {
            invln_contactexternalid = userGlobalId.ToString(),
            invln_contact = contactDtoSerialized,
        };

        await _serviceClient.ExecuteAsync(req, cancellationToken);
    }
}
