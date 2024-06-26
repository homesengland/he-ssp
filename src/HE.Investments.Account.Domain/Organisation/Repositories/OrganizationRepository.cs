using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Contract.Organisation;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Domain.Organisation.Entities;
using HE.Investments.Account.Domain.Organisation.Mappers;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.User;
using HE.Investments.Organisation.Services;

namespace HE.Investments.Account.Domain.Organisation.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly IOrganizationService _organizationService;

    private readonly IUserContext _userContext;

    private readonly InvestmentPartnerStatusMapper _investmentPartnerStatusMapper = new();

    private readonly IEventDispatcher _eventDispatcher;

    public OrganizationRepository(IOrganizationService organizationService, IUserContext userContext, IEventDispatcher eventDispatcher)
    {
        _organizationService = organizationService;
        _userContext = userContext;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<OrganizationBasicInformation> GetBasicInformation(OrganisationId organisationId, CancellationToken cancellationToken)
    {
        var organizationDetailsDto = await _organizationService.GetOrganizationDetails(organisationId.ToGuidAsString(), _userContext.UserGlobalId)
                                        ?? throw new NotFoundException(nameof(OrganizationBasicInformation), organisationId);

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
                organizationDetailsDto.compayAdminContactEmail),
            _investmentPartnerStatusMapper.ToDomain(organizationDetailsDto.investmentPartnerStatus)!.Value);
    }

    public async Task<OrganisationChangeRequestState> GetOrganisationChangeRequestDetails(OrganisationId organisationId, CancellationToken cancellationToken)
    {
        var response = await _organizationService.GetOrganisationChangeDetailsRequestContact(organisationId.ToGuidAsString());

        if (response == null)
        {
            return OrganisationChangeRequestState.NoPendingRequest;
        }

        if (response.contactExternalId == _userContext.UserGlobalId)
        {
            return OrganisationChangeRequestState.PendingRequestByYou;
        }

        return OrganisationChangeRequestState.PendingRequestByOthers;
    }

    public async Task<OrganisationId> CreateOrganisation(OrganisationEntity organisation)
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

        return await Task.FromResult(OrganisationId.From(id));
    }

    public async Task Save(OrganisationId organisationId, OrganisationEntity organisation, CancellationToken cancellationToken)
    {
        await _organizationService.CreateOrganisationChangeRequest(
            new OrganizationDetailsDto
            {
                organisationId = organisationId.ToGuidAsString(),
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
            _userContext.UserGlobalId);
    }

    public async Task DispatchEvents(DomainEntity domainEntity, CancellationToken cancellationToken)
    {
        await _eventDispatcher.Publish(domainEntity, cancellationToken);
    }
}
