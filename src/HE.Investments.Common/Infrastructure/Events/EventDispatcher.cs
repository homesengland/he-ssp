using HE.Investments.Common.Domain;
using MediatR;

namespace HE.Investments.Common.Infrastructure.Events;

public class EventDispatcher : IEventDispatcher
{
    private readonly IMediator _mediator;

    public EventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Publish(DomainEntity domainEntity, CancellationToken cancellationToken)
    {
        var domainEventsToPublish = domainEntity.GetDomainEventsAndRemove();

        foreach (var domainEvent in domainEventsToPublish)
        {
            await Publish(domainEvent, cancellationToken);
        }
    }

    public async Task Publish<TEvent>(TEvent domainEvent, CancellationToken cancellationToken)
        where TEvent : IDomainEvent
    {
        await _mediator.Publish(domainEvent, cancellationToken);
    }
}
