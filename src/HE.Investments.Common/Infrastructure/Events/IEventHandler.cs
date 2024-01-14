using System.Diagnostics.CodeAnalysis;
using HE.Investments.Common.Contract.Infrastructure.Events;
using MediatR;

namespace HE.Investments.Common.Infrastructure.Events;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : DomainEvent
{
    new Task Handle(TEvent domainEvent, CancellationToken cancellationToken);
}
