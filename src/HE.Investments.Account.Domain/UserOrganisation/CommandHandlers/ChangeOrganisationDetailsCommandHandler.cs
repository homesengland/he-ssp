extern alias Org;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Utils.Constants.Notification;
using HE.InvestmentLoans.Contract;
using HE.InvestmentLoans.Contract.UserOrganisation.Commands;
using HE.Investments.Account.Domain.Organisation.Entities;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investments.Account.Domain.UserOrganisation.CommandHandlers;

public class ChangeOrganisationDetailsCommandHandler : IRequestHandler<ChangeOrganisationDetailsCommand, OperationResult>
{
    private readonly ILoanUserContext _loanUserContext;
    private readonly IOrganizationRepository _repository;
    private readonly INotificationService _notificationService;

    public ChangeOrganisationDetailsCommandHandler(
        ILoanUserContext loanUserContext,
        IOrganizationRepository repository,
        INotificationService notificationService)
    {
        _loanUserContext = loanUserContext;
        _repository = repository;
        _notificationService = notificationService;
    }

    public async Task<OperationResult> Handle(ChangeOrganisationDetailsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userAccount = await _loanUserContext.GetSelectedAccount();

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

            await _repository.Save(organisation, userAccount, cancellationToken);

            await _notificationService.NotifySuccess(NotificationBodyType.ChangeOrganisationDetailsRequest, null);

            return OperationResult.Success();
        }
        catch (DomainValidationException domainValidationException)
        {
            return domainValidationException.OperationResult;
        }
    }
}
