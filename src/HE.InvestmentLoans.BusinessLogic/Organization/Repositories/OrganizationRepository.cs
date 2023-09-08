extern alias Org;

using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using HE.InvestmentLoans.CRM.Model;
using Microsoft.PowerPlatform.Dataverse.Client;
using Org::HE.Investments.Organisation.Services;

namespace HE.InvestmentLoans.BusinessLogic.Organization.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    private readonly IOrganizationService _organizationService;

    public OrganizationRepository(IOrganizationServiceAsync2 serviceClient, IOrganizationService organizationService)
    {
        _serviceClient = serviceClient;
        _organizationService = organizationService;
    }

    public async Task<OrganizationBasicInformation> GetBasicInformation(UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organizationDetailsDto = await _organizationService.GetOrganizationDetails(userAccount.AccountId.ToString()!, userAccount.UserGlobalId.ToString())
                                        ?? throw new NotFoundException(nameof(OrganizationBasicInformation), userAccount.AccountId.ToString()!);

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
