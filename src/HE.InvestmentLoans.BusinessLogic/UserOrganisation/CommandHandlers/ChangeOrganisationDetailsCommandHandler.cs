extern alias Org;

using HE.InvestmentLoans.BusinessLogic.Organization.Entities;
using HE.InvestmentLoans.BusinessLogic.Organization.Repositories;
using HE.InvestmentLoans.BusinessLogic.Organization.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract;
using HE.InvestmentLoans.Contract.UserOrganisation.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.UserOrganisation.CommandHandlers;

public class ChangeOrganisationDetailsCommandHandler : IRequestHandler<ChangeOrganisationDetailsCommand, OperationResult>
{
    private readonly ILoanUserContext _loanUserContext;
    private readonly IOrganizationRepository _repository;

    public ChangeOrganisationDetailsCommandHandler(ILoanUserContext loanUserContext, IOrganizationRepository repository)
    {
        _loanUserContext = loanUserContext;
        _repository = repository;
    }

    public async Task<OperationResult> Handle(ChangeOrganisationDetailsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!await _loanUserContext.IsLinkedWithOrganization())
            {
                throw new DomainException(
                    $"User with id {_loanUserContext.UserGlobalId} is not linked with organization: {request.Name}",
                    CommonErrorCodes.ContactIsNotLinkedWithRequestedOrganization);
            }

            var operationResult = OperationResult.New();
            var name = operationResult.Aggregate(() => new OrganisationName(
                request.Name,
                OrganisationErrorMessages.MissingRegisteredOrganisationName,
                FieldNameForInputLengthValidation.RequestToChangeOrganisationDetails));
            var phoneNumber = operationResult.Aggregate(() =>
                new OrganisationPhoneNumber(request.PhoneNumber, FieldNameForInputLengthValidation.RequestToChangeOrganisationDetails));
            var address = operationResult.Aggregate(() =>
                new OrganisationAddress(
                    request.AddressLine1,
                    request.AddressLine2,
                    null,
                    request.TownOrCity,
                    request.Postcode,
                    request.County,
                    null,
                    FieldNameForInputLengthValidation.RequestToChangeOrganisationDetails));

            operationResult.CheckErrors();

            var organisation = new OrganisationEntity(name, address, phoneNumber);

            await _repository.Update(organisation, cancellationToken);

            return OperationResult.Success();
        }
        catch (DomainValidationException domainValidationException)
        {
            return domainValidationException.OperationResult;
        }
    }
}
