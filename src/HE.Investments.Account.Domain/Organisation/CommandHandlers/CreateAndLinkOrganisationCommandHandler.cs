using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract;
using HE.Investments.Account.Contract.Organisation.Commands;
using HE.Investments.Account.Domain.Organisation.Entities;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using MediatR;
using IContactRepository = HE.Investments.Account.Domain.Organisation.Repositories.IContactRepository;

namespace HE.Investments.Account.Domain.Organisation.CommandHandlers;

public class CreateAndLinkOrganisationCommandHandler : IRequestHandler<CreateAndLinkOrganisationCommand, OperationResult>
{
    private readonly IAccountUserContext _loanUserContext;
    private readonly IOrganizationRepository _repository;
    private readonly IContactRepository _contactRepository;

    public CreateAndLinkOrganisationCommandHandler(
        IAccountUserContext loanUserContext,
        IOrganizationRepository repository,
        IContactRepository contactRepository)
    {
        _loanUserContext = loanUserContext;
        _repository = repository;
        _contactRepository = contactRepository;
    }

    public async Task<OperationResult> Handle(CreateAndLinkOrganisationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (await _loanUserContext.IsLinkedWithOrganisation())
            {
                throw new DomainException(
                    $"Cannot link organization to user account id: {_loanUserContext.UserGlobalId}, because it is already linked to other organization.",
                    CommonErrorCodes.ContactAlreadyLinkedWithOrganization);
            }

            var operationResult = OperationResult.New();
            var name = operationResult.Aggregate(() => new OrganisationName(request.Name));
            var address = operationResult.Aggregate(() =>
                new OrganisationAddress(request.AddressLine1, request.AddressLine2, null, request.TownOrCity, request.Postcode, request.County, null));
            var organisation = operationResult.Aggregate(() => new OrganisationEntity(name, address));

            operationResult.CheckErrors();

            var organisationId = await _repository.CreateOrganisation(organisation);

            await _contactRepository.LinkOrganisation(organisationId.ToString(), PortalConstants.LoansPortalType);

            return OperationResult.Success();
        }
        catch (DomainValidationException domainValidationException)
        {
            return domainValidationException.OperationResult;
        }
    }
}
