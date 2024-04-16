using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery.Events;
using HE.Investment.AHP.Contract.HomeTypes.Events;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investment.AHP.Domain.Delivery.EventHandlers;

public class MarkDeliveryAsInProgressEventHandler :
    IEventHandler<DeliveryPhaseHasBeenCreatedEvent>,
    IEventHandler<DeliveryPhaseHasBeenUpdatedEvent>,
    IEventHandler<DeliveryPhaseHasBeenRemovedEvent>,
    IEventHandler<HomeTypeNumberOfHomesHasBeenUpdatedEvent>
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

    public async Task Handle(HomeTypeNumberOfHomesHasBeenUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await ChangeStatus(domainEvent.ApplicationId, cancellationToken);
    }

    private async Task ChangeStatus(AhpApplicationId applicationId, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var deliveryPhases = await _repository.GetByApplicationId(applicationId, account, cancellationToken);

        if (deliveryPhases.Status.IsIn(SectionStatus.NotStarted))
        {
            return;
        }

        deliveryPhases.MarkAsInProgress();

        await _repository.Save(deliveryPhases, account.SelectedOrganisationId(), cancellationToken);
    }
}
