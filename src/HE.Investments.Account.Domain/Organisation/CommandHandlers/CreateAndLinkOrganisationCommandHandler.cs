using HE.Investments.Account.Contract.Organisation.Commands;
using HE.Investments.Account.Contract.User.Events;
using HE.Investments.Account.Domain.Organisation.Entities;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Errors;
using MediatR;
using IContactRepository = HE.Investments.Account.Domain.Organisation.Repositories.IContactRepository;

namespace HE.Investments.Account.Domain.Organisation.CommandHandlers;

public class CreateAndLinkOrganisationCommandHandler : IRequestHandler<CreateAndLinkOrganisationCommand, OperationResult>
{
    private readonly IAccountUserContext _userContext;
    private readonly IOrganizationRepository _repository;
    private readonly IContactRepository _contactRepository;
    private readonly IMediator _mediator;

    public CreateAndLinkOrganisationCommandHandler(
        IAccountUserContext userContext,
        IOrganizationRepository repository,
        IContactRepository contactRepository,
        IMediator mediator)
    {
        _userContext = userContext;
        _repository = repository;
        _contactRepository = contactRepository;
        _mediator = mediator;
    }

    public async Task<OperationResult> Handle(CreateAndLinkOrganisationCommand request, CancellationToken cancellationToken)
    {
        if (await _userContext.IsLinkedWithOrganisation())
        {
            throw new DomainException(
                $"Cannot link organization to user account id: {_userContext.UserGlobalId}, because it is already linked to other organization.",
                CommonErrorCodes.ContactAlreadyLinkedWithOrganization);
        }

        var operationResult = OperationResult.New();
        var name = operationResult.Aggregate(() => new OrganisationName(request.Name));
        var address = operationResult.Aggregate(() =>
            new OrganisationAddress(request.AddressLine1, request.AddressLine2, null, request.TownOrCity, request.Postcode, request.County, null));
        var organisation = operationResult.Aggregate(() => new OrganisationEntity(name, address));

        operationResult.CheckErrors();

        var organisationId = await _repository.CreateOrganisation(organisation);

        await _contactRepository.LinkOrganisation(organisationId, PortalConstants.CommonPortalType);
        await _mediator.Publish(new UserAccountsChangedEvent(_userContext.UserGlobalId), cancellationToken);

        return OperationResult.Success();
    }
}
