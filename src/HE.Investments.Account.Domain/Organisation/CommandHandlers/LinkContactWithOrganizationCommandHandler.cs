using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract;
using HE.Investments.Account.Contract.Organisation.Commands;
using HE.Investments.Account.Shared;
using HE.Investments.Organisation.Contract;
using HE.Investments.Organisation.Services;
using MediatR;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Account.Domain.Organisation.CommandHandlers;

public class LinkContactWithOrganizationCommandHandler : IRequestHandler<LinkContactWithOrganizationCommand>
{
    private readonly IAccountUserContext _loanUserContext;
    private readonly IOrganizationService _organizationService;
    private readonly IOrganisationSearchService _organisationSearchService;
    private readonly IOrganizationServiceAsync2 _organizationServiceAsync;
    private readonly IContactService _contactService;

    public LinkContactWithOrganizationCommandHandler(IAccountUserContext loanUserContext, IOrganizationService organizationService, IOrganisationSearchService organisationSearchService, IOrganizationServiceAsync2 organizationServiceAsync, IContactService contactService)
    {
        _loanUserContext = loanUserContext;
        _organizationService = organizationService;
        _organisationSearchService = organisationSearchService;
        _organizationServiceAsync = organizationServiceAsync;
        _contactService = contactService;
    }

    public async Task Handle(LinkContactWithOrganizationCommand request, CancellationToken cancellationToken)
    {
        if (await _loanUserContext.IsLinkedWithOrganisation())
        {
            throw new DomainException(
                $"Cannot link organization id: {request.CompaniesHouseNumber} to loan user account id: {_loanUserContext.UserGlobalId}, because it is already linked to other organization",
                CommonErrorCodes.ContactAlreadyLinkedWithOrganization);
        }

        var result = await _organisationSearchService.GetByOrganisation(request.CompaniesHouseNumber, cancellationToken);

        if (!result.IsSuccessfull())
        {
            throw new ExternalServiceException();
        }

        var organization = result.Item ?? throw new NotFoundException(nameof(OrganisationSearchItem), request.CompaniesHouseNumber);

        if (!organization.ExistsInCrm)
        {
            _organizationService.CreateOrganization(new OrganizationDetailsDto
            {
                registeredCompanyName = organization.Name,
                addressLine1 = organization.Street,
                city = organization.City,
                companyRegistrationNumber = request.CompaniesHouseNumber,
                postalcode = organization.PostalCode,
            });
        }

        await _contactService.LinkContactWithOrganization(_organizationServiceAsync, _loanUserContext.UserGlobalId.ToString(), request.CompaniesHouseNumber, PortalConstants.LoansPortalType);
        await _loanUserContext.RefreshAccounts();
    }
}
