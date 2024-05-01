using HE.Investments.Account.Contract.Organisation.Commands;
using HE.Investments.Account.Contract.User.Events;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Errors;
using HE.Investments.Organisation.Services;
using HE.Investments.Organisation.ValueObjects;
using MediatR;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Account.Domain.Organisation.CommandHandlers;

public class LinkContactWithOrganizationCommandHandler : IRequestHandler<LinkContactWithOrganisationCommand>
{
    private readonly IAccountUserContext _userContext;
    private readonly IOrganizationServiceAsync2 _organizationServiceAsync;
    private readonly IContactService _contactService;
    private readonly IMediator _mediator;
    private readonly IInvestmentsOrganisationService _organisationService;

    public LinkContactWithOrganizationCommandHandler(
        IAccountUserContext userContext,
        IOrganizationServiceAsync2 organizationServiceAsync,
        IContactService contactService,
        IMediator mediator,
        IInvestmentsOrganisationService organisationService)
    {
        _userContext = userContext;
        _organizationServiceAsync = organizationServiceAsync;
        _contactService = contactService;
        _mediator = mediator;
        _organisationService = organisationService;
    }

    public async Task Handle(LinkContactWithOrganisationCommand request, CancellationToken cancellationToken)
    {
        if (await _userContext.IsLinkedWithOrganisation())
        {
            throw new DomainException(
                $"Cannot link organization id: {request.CompaniesHouseNumber} to loan user account id: {_userContext.UserGlobalId}, because it is already linked to other organization",
                CommonErrorCodes.ContactAlreadyLinkedWithOrganization);
        }

        var organisation = await _organisationService.GetOrganisation(
            new OrganisationIdentifier(request.CompaniesHouseNumber),
            cancellationToken);

        await _contactService.LinkContactWithOrganization(
            _organizationServiceAsync,
            _userContext.UserGlobalId.ToString(),
            organisation.Id.ToString(),
            PortalConstants.CommonPortalType);

        await _mediator.Publish(new UserAccountsChangedEvent(_userContext.UserGlobalId), cancellationToken);
    }
}
