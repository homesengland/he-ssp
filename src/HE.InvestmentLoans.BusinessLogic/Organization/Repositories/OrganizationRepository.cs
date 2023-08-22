using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using HE.InvestmentLoans.CRM.Model;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.InvestmentLoans.BusinessLogic.Organization.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    public OrganizationRepository(IOrganizationServiceAsync2 serviceClient)
    {
        _serviceClient = serviceClient;
    }

    public async Task<OrganizationBasicInformation> GetBasicInformation(UserAccount userAccount, CancellationToken cancellationToken)
    {
        var request = new invln_getorganizationdetailsRequest()
        {
            invln_contactexternalid = userAccount.UserGlobalId,
            invln_accountid = userAccount.AccountId.ToString(),
        };

        var response = (invln_getorganizationdetailsResponse)await _serviceClient.ExecuteAsync(request);
        var organizationDetailsDto = CrmResponseSerializer.Deserialize<OrganizationDetailsDto>(response.invln_organizationdetails)!;
        return new OrganizationBasicInformation(
            organizationDetailsDto.registeredCompanyName,
            organizationDetailsDto.companyRegistrationNumber,
            new Address(
                organizationDetailsDto.addressLine1,
                organizationDetailsDto.addressLine2,
                organizationDetailsDto.addressLine3,
                organizationDetailsDto.city,
                organizationDetailsDto.postalcode,
                organizationDetailsDto.country));
    }
}
