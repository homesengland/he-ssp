using HE.Investments.Account.Contract.Organisation.Commands;
using HE.Investments.Account.Contract.User.Events;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Organisation.Services;
using HE.Investments.Organisation.ValueObjects;
using MediatR;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Account.Domain.Organisation.CommandHandlers;

public class LinkContactWithOrganizationCommandHandler : IRequestHandler<LinkContactWithOrganisationCommand, OperationResult>
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

    public async Task<OperationResult> Handle(LinkContactWithOrganisationCommand request, CancellationToken cancellationToken)
    {
        if (request.IsConfirmed.IsNotProvided())
        {
            OperationResult.ThrowValidationError(
                nameof(request.IsConfirmed),
                ValidationErrorMessage.ChooseYourAnswer);
        }

        var userAccounts = await _userContext.GetAccounts();

        var organisationToBeLinked = await _organisationService.GetOrganisation(
            new OrganisationIdentifier(request.CompaniesHouseNumber),
            cancellationToken);

        if (userAccounts != null && userAccounts.Any(x => x.Organisation?.OrganisationId == organisationToBeLinked.Id))
        {
            OperationResult.ThrowValidationError(
                nameof(request.IsConfirmed),
                $"You are already linked with {organisationToBeLinked.Name}");
        }

        await _contactService.LinkContactWithOrganization(
            _organizationServiceAsync,
            _userContext.UserGlobalId.ToString(),
            organisationToBeLinked.Id.ToGuidAsString(),
            PortalConstants.CommonPortalType);

        await _mediator.Publish(new UserAccountsChangedEvent(_userContext.UserGlobalId), cancellationToken);

        return OperationResult.Success();
    }
}
