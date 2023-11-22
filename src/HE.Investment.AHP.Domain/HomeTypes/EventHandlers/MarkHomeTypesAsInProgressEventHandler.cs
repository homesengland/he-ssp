using HE.Investment.AHP.Contract.HomeTypes.Events;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Common.Infrastructure.Events;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.HomeTypes.EventHandlers;

public class MarkHomeTypesAsInProgressEventHandler :
    IEventHandler<HomeTypeHasBeenCreatedEvent>,
    IEventHandler<HomeTypeHasBeenUpdatedEvent>,
    IEventHandler<HomeTypeHasBeenRemovedEvent>
{
    private readonly IHomeTypeRepository _repository;

    public MarkHomeTypesAsInProgressEventHandler(IHomeTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(HomeTypeHasBeenCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await ChangeStatus(domainEvent.ApplicationId, cancellationToken);
    }

    public async Task Handle(HomeTypeHasBeenUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await ChangeStatus(domainEvent.ApplicationId, cancellationToken);
    }

    public async Task Handle(HomeTypeHasBeenRemovedEvent domainEvent, CancellationToken cancellationToken)
    {
        await ChangeStatus(domainEvent.ApplicationId, cancellationToken);
    }

    private async Task ChangeStatus(string applicationId, CancellationToken cancellationToken)
    {
        var homeTypes = await _repository.GetByApplicationId(new ApplicationId(applicationId), HomeTypeSegmentTypes.None, cancellationToken);
        homeTypes.MarkAsInProgress();

        await _repository.Save(homeTypes, cancellationToken);
    }
}
