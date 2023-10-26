using HE.InvestmentLoans.Common.Domain;
using MediatR;

namespace HE.InvestmentLoans.Common.Events;

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
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}
