using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Extensions;
using HE.Investments.Organisation.ValueObjects;

namespace HE.Investments.Organisation.Services;

public class InvestmentsOrganisationService : IInvestmentsOrganisationService
{
    private readonly IOrganisationSearchService _organisationSearchService;

    private readonly IOrganizationService _organisationService;

    public InvestmentsOrganisationService(IOrganisationSearchService organisationSearchService, IOrganizationService organisationService)
    {
        _organisationSearchService = organisationSearchService;
        _organisationService = organisationService;
    }

    public async Task<InvestmentsOrganisation> GetOrganisation(OrganisationIdentifier organisationIdentifier, CancellationToken cancellationToken)
    {
        var result = await _organisationSearchService.GetByOrganisation(organisationIdentifier.Value, cancellationToken);
        if (!result.IsSuccessfull())
        {
            throw new ExternalServiceException();
        }

        var organisation = result.Item ?? throw new NotFoundException("Organisation", organisationIdentifier);
        if (organisation.OrganisationId.IsProvided())
        {
            return new InvestmentsOrganisation(new OrganisationId(organisation.OrganisationId!), organisation.Name);
        }

        var organisationDto = new OrganizationDetailsDto
        {
            registeredCompanyName = organisation.Name,
            addressLine1 = organisation.Street,
            city = organisation.City,
            companyRegistrationNumber = result.Item.CompanyNumber,
            postalcode = organisation.PostalCode,
        };
        var organisationId = new OrganisationId(_organisationService.CreateOrganization(organisationDto).ToString());

        return new InvestmentsOrganisation(organisationId, organisation.Name);
    }
}
