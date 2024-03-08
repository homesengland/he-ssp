using HE.Investments.Account.Contract.UserOrganisation.Commands;
using HE.Investments.Account.Domain.Organisation.Entities;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Account.Domain.UserOrganisation.Notifications;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Errors;
using HE.Investments.Common.Messages;
using MediatR;
using INotificationPublisher = HE.Investments.Common.Services.Notifications.INotificationPublisher;

namespace HE.Investments.Account.Domain.UserOrganisation.CommandHandlers;

public class ChangeOrganisationDetailsCommandHandler : IRequestHandler<ChangeOrganisationDetailsCommand, OperationResult>
{
    private readonly IAccountUserContext _accountUserContext;
    private readonly IOrganizationRepository _repository;
    private readonly INotificationPublisher _notificationPublisher;

    public ChangeOrganisationDetailsCommandHandler(
        IAccountUserContext accountUserContext,
        IOrganizationRepository repository,
        INotificationPublisher notificationPublisher)
    {
        _accountUserContext = accountUserContext;
        _repository = repository;
        _notificationPublisher = notificationPublisher;
    }

    public async Task<OperationResult> Handle(ChangeOrganisationDetailsCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();

        if (!await _accountUserContext.IsLinkedWithOrganisation())
        {
            throw new DomainException(
                $"User with id {_accountUserContext.UserGlobalId} is not linked with organization: {request.Name}",
                CommonErrorCodes.ContactIsNotLinkedWithRequestedOrganization);
        }

        var operationResult = OperationResult.New();
        var name = operationResult.Aggregate(() => new OrganisationName(request.Name, OrganisationErrorMessages.MissingRegisteredOrganisationName));
        var phoneNumber = operationResult.Aggregate(() => new OrganisationPhoneNumber(request.PhoneNumber));
        var address = operationResult.Aggregate(() =>
            new OrganisationAddress(
                request.AddressLine1,
                request.AddressLine2,
                null,
                request.TownOrCity,
                request.Postcode,
                request.County,
                null));

        operationResult.CheckErrors();

        var organisation = new OrganisationEntity(name, address, phoneNumber);

        await _repository.Save(userAccount.SelectedOrganisationId(), organisation, cancellationToken);

        await _notificationPublisher.Publish(new ChangeOrganisationDetailsRequestedNotification());

        return OperationResult.Success();
    }
}
