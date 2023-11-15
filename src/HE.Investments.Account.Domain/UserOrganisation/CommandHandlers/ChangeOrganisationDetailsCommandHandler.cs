using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract;
using HE.Investments.Account.Contract.UserOrganisation.Commands;
using HE.Investments.Account.Domain.Organisation.Entities;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Account.Domain.UserOrganisation.Notifications;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investments.Account.Domain.UserOrganisation.CommandHandlers;

public class ChangeOrganisationDetailsCommandHandler : IRequestHandler<ChangeOrganisationDetailsCommand, OperationResult>
{
    private readonly IAccountUserContext _accountUserContext;
    private readonly IOrganizationRepository _repository;
    private readonly INotificationService _notificationService;

    public ChangeOrganisationDetailsCommandHandler(
        IAccountUserContext accountUserContext,
        IOrganizationRepository repository,
        INotificationService notificationService)
    {
        _accountUserContext = accountUserContext;
        _repository = repository;
        _notificationService = notificationService;
    }

    public async Task<OperationResult> Handle(ChangeOrganisationDetailsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userAccount = await _accountUserContext.GetSelectedAccount();

            if (!await _accountUserContext.IsLinkedWithOrganisation())
            {
                throw new DomainException(
                    $"User with id {_accountUserContext.UserGlobalId} is not linked with organization: {request.Name}",
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

            await _notificationService.Publish(new ChangeOrganisationDetailsRequestedNotification());

            return OperationResult.Success();
        }
        catch (DomainValidationException domainValidationException)
        {
            return domainValidationException.OperationResult;
        }
    }
}
