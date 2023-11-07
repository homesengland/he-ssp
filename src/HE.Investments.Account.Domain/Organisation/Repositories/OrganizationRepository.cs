extern alias Org;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Domain.Organisation.Entities;
using HE.Investments.Account.Shared.User;
using HE.Investments.Organisation.Services;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Account.Domain.Organisation.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly IOrganizationService _organizationService;

    public OrganizationRepository(IOrganizationService organizationService)
    {
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
        var accountId = userAccount.AccountId ?? throw new NotFoundException(nameof(userAccount.AccountId));

        var response = await _organizationService.GetOrganisationChangeDetailsRequestContact(accountId);

        if (response == null)
        {
            return OrganisationChangeRequestState.NoPendingRequest;
        }

        if (response.contactExternalId == userAccount.UserGlobalId.ToString())
        {
            return OrganisationChangeRequestState.PendingRequestByYou;
        }

        return OrganisationChangeRequestState.PendingRequestByOthers;
    }

    public async Task<Guid> CreateOrganisation(OrganisationEntity organisation)
    {
        var id = _organizationService.CreateOrganization(new OrganizationDetailsDto
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

    public async Task Save(OrganisationEntity organisation, UserAccount userAccount, CancellationToken cancellationToken)
    {
        await _organizationService.CreateOrganisationChangeRequest(
            new OrganizationDetailsDto
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
