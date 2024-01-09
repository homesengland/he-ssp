using System.Diagnostics.CodeAnalysis;
using HE.Investments.Common.Contract.Infrastructure.Events;
using MediatR;

namespace HE.Investments.Common.Infrastructure.Events;

[SuppressMessage("Naming", "CA1711", Justification = "It is part of infrastructure")]
public interface IEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : DomainEvent
{
    new Task Handle(TEvent domainEvent, CancellationToken cancellationToken);
}
