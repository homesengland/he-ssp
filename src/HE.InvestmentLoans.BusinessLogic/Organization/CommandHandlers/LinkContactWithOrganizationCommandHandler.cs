extern alias Org;

using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.Contract.Organization;
using MediatR;
using Microsoft.PowerPlatform.Dataverse.Client;
using Org.HE.Investments.Organisation.Contract;
using Org.HE.Investments.Organisation.Services;

namespace HE.InvestmentLoans.BusinessLogic.Organization.CommandHandlers;
public class LinkContactWithOrganizationCommandHandler : IRequestHandler<LinkContactWithOrganizationCommand>
{
    private readonly ILoanUserContext _loanUserContext;
    private readonly IOrganizationService _organizationService;
    private readonly IOrganisationSearchService _organisationSearchService;
    private readonly IOrganizationServiceAsync2 _organizationServiceAsync;
    private readonly IContactService _contactService;

    public LinkContactWithOrganizationCommandHandler(ILoanUserContext loanUserContext, IOrganizationService organizationService, IOrganisationSearchService organisationSearchService, IOrganizationServiceAsync2 organizationServiceAsync, IContactService contactService)
    {
        _loanUserContext = loanUserContext;
        _organizationService = organizationService;
        _organisationSearchService = organisationSearchService;
        _organizationServiceAsync = organizationServiceAsync;
        _contactService = contactService;
    }

    public async Task Handle(LinkContactWithOrganizationCommand request, CancellationToken cancellationToken)
    {
        if (await _loanUserContext.IsLinkedWithOrganization())
        {
            throw new DomainException($"Cannot link organization id: {request.Number} to loan user account id: {_loanUserContext.UserGlobalId}, because it is already linked to other organization", string.Empty);
        }

        var result = await _organisationSearchService.GetByCompaniesHouseNumber(request.Number.ToString(), cancellationToken);

        if (!result.IsSuccessfull())
        {
            throw new ExternalServiceException();
        }

        var organization = result.Item ?? throw new NotFoundException(nameof(OrganisationSearchItem), request.Number);

        if (!organization.ExistsInCrm)
        {
            _organizationService.CreateOrganization(new Org.HE.Common.IntegrationModel.PortalIntegrationModel.OrganizationDetailsDto
            {
                registeredCompanyName = organization.Name,
                addressLine1 = organization.Street,
                city = organization.City,
                companyRegistrationNumber = request.Number.ToString(),
                postalcode = organization.PostalCode,
            });
        }

        await _contactService.LinkContactWithOrganization(_organizationServiceAsync, _loanUserContext.UserGlobalId.ToString(), request.Number.ToString()!, PortalConstants.PortalType);
    }
}
