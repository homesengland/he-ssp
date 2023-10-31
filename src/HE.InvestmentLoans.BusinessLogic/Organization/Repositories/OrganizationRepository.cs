extern alias Org;

using HE.InvestmentLoans.BusinessLogic.Organization.Entities;
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
        if (!Guid.TryParse(userAccount.AccountId.ToString(), out var accountId))
        {
            throw new NotFoundException(nameof(OrganisationEntity), accountId);
        }

        var response = await _organizationService.GetOrganisationChangeDetailsRequestContact(accountId);

        if (response == null)
        {
            return OrganisationChangeRequestState.NoPendingRequest;
        }
        else if (response.contactExternalId == userAccount.UserGlobalId.ToString())
        {
            return OrganisationChangeRequestState.PendingRequestByYou;
        }
        else
        {
            return OrganisationChangeRequestState.PendingRequestByOthers;
        }
    }

    public async Task<Guid> CreateOrganisation(OrganisationEntity organisation)
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

    public async Task Update(OrganisationEntity organisation, UserAccount userAccount, CancellationToken cancellationToken)
    {
        await _organizationService.CreateOrganisationChangeRequest(
            new Org.HE.Common.IntegrationModel.PortalIntegrationModel.OrganizationDetailsDto
            {
                organisationId = userAccount.AccountId.ToString(),
                registeredCompanyName = organisation.Name.ToString(),
                organisationPhoneNumber = organisation.PhoneNumber.ToString(),
                addressLine1 = organisation.Address.AddressLine1,
                addressLine2 = organisation.Address.AddressLine2,
                addressLine3 = organisation.Address.AddressLine3,
                city = organisation.Address.TownOrCity,
                country = organisation.Address.County,
                postalcode = organisation.Address.Postcode.Value,
                county = organisation.Address.County,
            },
            userAccount.UserGlobalId.ToString());
    }
}
