extern alias Org;

using HE.InvestmentLoans.BusinessLogic.Organization.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
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
                organizationDetailsDto.country),
            new ContactInformation(
                organizationDetailsDto.compayAdminContactTelephone,
                organizationDetailsDto.compayAdminContactEmail));
    }

    public async Task<OrganisationChangeRequestState> GetOrganisationChangeRequestDetails(UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (!userAccount.AccountId.HasValue)
        {
            return OrganisationChangeRequestState.NoPendingRequest;
        }

        var changeRequestDetails = await _organizationService.GetOrganisationChangeDetailsRequest(userAccount.AccountId.Value);

        // TODO: to be updated when organisation service start returning final results
        switch (changeRequestDetails)
        {
            case "You requested":
                return OrganisationChangeRequestState.PendingRequestByYou;
            case "No Request":
                return OrganisationChangeRequestState.NoPendingRequest;
            case "Pending request":
                return OrganisationChangeRequestState.PendingRequestByOthers;
            default:
                break;
        }

        throw new ArgumentOutOfRangeException(changeRequestDetails, nameof(changeRequestDetails) + "has incorrect value!");
    }

    public async Task<Guid> CreateOrganisation(OrganisationToCreate organisation)
    {
        var id = _organizationService.CreateOrganization(new Org.HE.Common.IntegrationModel.PortalIntegrationModel.OrganizationDetailsDto
        {
            registeredCompanyName = organisation.Name.ToString(),
            addressLine1 = organisation.Address.AddressLine1,
            addressLine2 = organisation.Address.AddressLine2,
            addressLine3 = organisation.Address.AddressLine3,
            city = organisation.Address.TownOrCity,
            country = organisation.Address.County,
            postalcode = organisation.Address.Postcode.Value,
            county = organisation.Address.County,
        });

        return await Task.FromResult(id);
    }
}
