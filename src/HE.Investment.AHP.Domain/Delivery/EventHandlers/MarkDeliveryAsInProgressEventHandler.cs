using HE.Investment.AHP.Contract.Delivery.Events;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Infrastructure.Events;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Delivery.EventHandlers;

public class MarkDeliveryAsInProgressEventHandler :
    IEventHandler<DeliveryPhaseHasBeenCreatedEvent>,
    IEventHandler<DeliveryPhaseHasBeenUpdatedEvent>,
    IEventHandler<DeliveryPhaseHasBeenRemovedEvent>
{
    private readonly IDeliveryPhaseRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public MarkDeliveryAsInProgressEventHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task Handle(DeliveryPhaseHasBeenCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await ChangeStatus(domainEvent.ApplicationId, cancellationToken);
    }

    public async Task Handle(DeliveryPhaseHasBeenUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await ChangeStatus(domainEvent.ApplicationId, cancellationToken);
    }

    public async Task Handle(DeliveryPhaseHasBeenRemovedEvent domainEvent, CancellationToken cancellationToken)
    {
        await ChangeStatus(domainEvent.ApplicationId, cancellationToken);
    }

    private async Task ChangeStatus(string applicationId, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var deliveryPhases = await _repository.GetByApplicationId(new ApplicationId(applicationId), account, cancellationToken);
        deliveryPhases.MarkAsInProgress();

        await _repository.Save(deliveryPhases, account.SelectedOrganisationId(), cancellationToken);
    }
}
