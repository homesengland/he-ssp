extern alias Org;

using HE.InvestmentLoans.BusinessLogic.Organization.Repositories;
using HE.InvestmentLoans.BusinessLogic.Organization.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract;
using HE.InvestmentLoans.Contract.Organization.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Organization.CommandHandlers;

public class CreateAndLinkOrganisationCommandHandler : IRequestHandler<CreateAndLinkOrganisationCommand, OperationResult>
{
    private readonly ILoanUserContext _loanUserContext;
    private readonly IOrganizationRepository _repository;
    private readonly IContactRepository _contactRepository;

    public CreateAndLinkOrganisationCommandHandler(
        ILoanUserContext loanUserContext,
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
            if (await _loanUserContext.IsLinkedWithOrganization())
            {
                throw new DomainException(
                    $"Cannot link organization to user account id: {_loanUserContext.UserGlobalId}, because it is already linked to other organization.",
                    CommonErrorCodes.ContactAlreadyLinkedWithOrganization);
            }

            var operationResult = OperationResult.New();
            var postcode = operationResult.Aggregate(() => new Postcode(request.Postcode));
            var address = operationResult.Aggregate(() =>
                new OrganisationAddress(request.AddressLine1, request.AddressLine2, null, request.TownOrCity, postcode, request.County, null));
            var organisation = operationResult.Aggregate(() => new OrganisationToCreate(request.Name, address));

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
